using Serilog;
using ApiGateway.Middlewares;
using ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.ConfigureSerilog();

// Configure Swagger
builder.ConfigureSwagger();

// Configure YARP Reverse Proxy
builder.ConfigureYarp();

// Configure Observability
builder.ConfigureObservability();

var app = builder.Build();

app.UseRouting();

// Setup Metrics & Prometheus
app.UseObservability();

app.UseMiddleware<GatewayLoggingMiddleware>();

// Setup Swagger UI & Routes
app.UseGatewaySwagger();
app.MapSwaggerRoutes();

// Setup YARP Reverse Proxy
app.UseYarp();

try
{
    Log.Information("Ứng dụng API Gateway đang khởi động...");

    // Kiểm tra kết nối MongoDB
    await app.CheckMongoAsync(builder.Configuration);

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

