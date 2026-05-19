using System.Net;
using Polly;
using Yarp.ReverseProxy.Forwarder;
using ApiGateway.Middlewares;
using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;
using Prometheus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() // Bắt buộc phải có dòng này để lấy dữ liệu từ LogContext.PushProperty
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ---------------------------------------------------------------------
// Cấu hình Polly Resilience Pipeline cho Outgoing Requests của Yarp
// ---------------------------------------------------------------------
var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
    // CHIẾN LƯỢC 1: RETRY (Thử lại khi gặp lỗi tạm thời)
    .AddRetry(new()
    {
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(r => r.StatusCode == HttpStatusCode.InternalServerError
                            || r.StatusCode == HttpStatusCode.BadGateway
                            || r.StatusCode == HttpStatusCode.ServiceUnavailable),
        MaxRetryAttempts = 3, // Thử lại tối đa 3 lần
        Delay = TimeSpan.FromSeconds(1), // Lần đầu đợi 1s
        BackoffType = DelayBackoffType.Exponential, // Đợi tăng dần theo cấp số nhân (1s -> 2s -> 4s)
        UseJitter = true // Thêm một chút thời gian ngẫu nhiên để tránh các lượt retry trùng nhau gây ngập lụt server
    })
    .AddCircuitBreaker(new()
    {
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
            .Handle<HttpRequestException>()
            .HandleResult(r => (int)r.StatusCode >= 500),

        FailureRatio = 0.5,
        SamplingDuration = TimeSpan.FromSeconds(10),

        MinimumThroughput = 8,

        BreakDuration = TimeSpan.FromSeconds(30)
    })
    .AddTimeout(TimeSpan.FromSeconds(5))
    .Build();

builder.Services.AddSingleton<IForwarderHttpClientFactory>(new ResilientForwarderHttpClientFactory(pipeline));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("ApiGateway"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"] ?? "http://localhost:4317");
            });
    });

var app = builder.Build();
app.UseRouting();
app.UseHttpMetrics();
app.UseMiddleware<GatewayLoggingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/nhansu/v1/swagger.json", "Nhân Sự Service");
    options.SwaggerEndpoint("/swagger/chamcong/v1/swagger.json", "Chấm Công Service");
    options.SwaggerEndpoint("/swagger/quyetdinh/v1/swagger.json", "Quyết Định Service");
    options.SwaggerEndpoint("/swagger/tinhluong/v1/swagger.json", "Tính Lương Service");
    options.RoutePrefix = "swagger"; // Truy cập Swagger tại http://localhost:8080/swagger
});

// Endpoint tự động gộp và biên dịch Swagger của từng microservice
app.MapGet("/swagger/{service}/v1/swagger.json", async (string service, HttpClient httpClient, IConfiguration configuration) =>
{
    string? targetUrl = service switch
    {
        "nhansu" => configuration["ReverseProxy:Clusters:nhansu-cluster:Destinations:destination1:Address"],
        "chamcong" => configuration["ReverseProxy:Clusters:chamcong-cluster:Destinations:destination1:Address"],
        "quyetdinh" => configuration["ReverseProxy:Clusters:quyetdinh-cluster:Destinations:destination1:Address"],
        "tinhluong" => configuration["ReverseProxy:Clusters:tinhluong-cluster:Destinations:destination1:Address"],
        _ => null
    };

    if (string.IsNullOrEmpty(targetUrl))
    {
        return Results.NotFound($"Không tìm thấy dịch vụ {service} trong cấu hình.");
    }

    try
    {
        string downstreamSwaggerUrl = $"{targetUrl.TrimEnd('/')}/swagger/v1/swagger.json";
        var jsonString = await httpClient.GetStringAsync(downstreamSwaggerUrl);

        var node = System.Text.Json.Nodes.JsonNode.Parse(jsonString);
        if (node is System.Text.Json.Nodes.JsonObject rootObj &&
            rootObj.TryGetPropertyValue("paths", out var pathsNode) &&
            pathsNode is System.Text.Json.Nodes.JsonObject pathsObj)
        {
            var newPaths = new System.Text.Json.Nodes.JsonObject();
            foreach (var property in pathsObj.ToList())
            {
                string newKey = property.Key;
                if (property.Key.StartsWith("/api/"))
                {
                    newKey = "/api/" + service + property.Key.Substring(4);
                }

                pathsObj.Remove(property.Key);
                newPaths.Add(newKey, property.Value);
            }
            rootObj["paths"] = newPaths;

            // Xóa trường servers (nếu có) để Swagger UI gửi request tương đối từ host Gateway
            rootObj.Remove("servers");
        }

        return Results.Content(node?.ToJsonString() ?? jsonString, "application/json", System.Text.Encoding.UTF8);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Không thể kết nối lấy tài liệu Swagger từ {service}: {ex.Message}");
    }
});

// Ánh xạ các tuyến đường của Reverse Proxy YARP
app.MapReverseProxy();
app.MapMetrics();

try
{
    Log.Information("Ứng dụng API Gateway đang khởi động...");

    // Kiểm tra kết nối MongoDB (sử dụng cấu hình từ Serilog)
    try
    {
        var mongoUrlStr = builder.Configuration["Serilog:WriteTo:1:Args:databaseUrl"];
        if (!string.IsNullOrEmpty(mongoUrlStr))
        {
            var mongoUrl = new MongoUrl(mongoUrlStr);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3); // Giới hạn thời gian kết nối là 3 giây để tránh làm nghẽn quá trình khởi chạy nếu MongoDB bị offline

            var client = new MongoClient(settings);
            // Thực hiện lệnh ping để xác nhận kết nối thực tế tới MongoDB
            await client.GetDatabase(mongoUrl.DatabaseName ?? "HrmLogsDb").RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            Log.Information("Kết nối thành công đến MongoDB ({DatabaseName}). Sẵn sàng ghi nhận traffic log.", mongoUrl.DatabaseName);
        }
        else
        {
            Log.Warning("Không tìm thấy cấu hình connection string của MongoDB trong Serilog.");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Không thể kết nối đến cơ sở dữ liệu MongoDB. Vui lòng kiểm tra trạng thái Docker Container.");
    }

    app.Run();
}
catch (Exception ex)
{
    // Phòng trường hợp YARP cấu hình sai file json gây crash ứng dụng khi start
    Log.Fatal(ex, "Ứng dụng API Gateway gặp lỗi nghiêm trọng khi khởi động.");
}
finally
{
    // Đảm bảo toàn bộ log còn tồn đọng trong bộ nhớ đệm (Buffer) được đẩy hết xuống MongoDB trước khi ứng dụng tắt hẳn
    Log.CloseAndFlush();
}



// ---------------------------------------------------------------------
// CÁC LỚP HỖ TRỢ ĐĂNG KÝ RESILIENCE CHO YARP FORWARDER HTTP CLIENT
// ---------------------------------------------------------------------
public class ResilientForwarderHttpClientFactory : ForwarderHttpClientFactory
{
    private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

    public ResilientForwarderHttpClientFactory(ResiliencePipeline<HttpResponseMessage> pipeline)
    {
        _pipeline = pipeline;
    }

    protected override HttpMessageHandler WrapHandler(ForwarderHttpClientContext context, HttpMessageHandler handler)
    {
        var baseHandler = base.WrapHandler(context, handler);
        return new PollyDelegatingHandler(_pipeline, baseHandler);
    }
}

public class PollyDelegatingHandler : DelegatingHandler
{
    private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;

    public PollyDelegatingHandler(ResiliencePipeline<HttpResponseMessage> pipeline, HttpMessageHandler innerHandler)
    {
        _pipeline = pipeline;
        InnerHandler = innerHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await _pipeline.ExecuteAsync(async token =>
            await base.SendAsync(request, token), cancellationToken);
    }
}
