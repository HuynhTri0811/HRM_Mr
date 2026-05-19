using System.Diagnostics;
using Serilog.Context;

namespace ApiGateway.Middlewares;

public class GatewayLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeaderKey = "X-Correlation-Id";

    public GatewayLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Kiểm tra xem Client có gửi mã TraceId/CorrelationId không, nếu không thì tự sinh mới
        if (!context.Request.Headers.TryGetValue(CorrelationHeaderKey, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        // 2. Đính mã này vào Header của Request hiện tại để các Backend Service phía sau nhận được
        context.Request.Headers[CorrelationHeaderKey] = correlationId;
        // Đính thêm vào Response Header để tiện cho việc kiểm tra/đối chiếu phía Client (nếu cần)
        context.Response.Headers[CorrelationHeaderKey] = correlationId;

        // Bắt đầu bấm giờ
        var stopwatch = Stopwatch.StartNew();

        // 3. Sử dụng LogContext để đẩy "TraceId" vào làm thuộc tính chung của đoạn Log này
        using (LogContext.PushProperty("TraceId", correlationId.ToString()))
        using (LogContext.PushProperty("ClientIp", context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"))
        using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
        {
            try
            {
                // Cho request đi tiếp qua Gateway để đến Backend Service
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                // 4. Khi request quay trở ra, tiến hành ghi log lưu vào MongoDB
                var requestPath = context.Request.Path;
                var httpMethod = context.Request.Method;
                var statusCode = context.Response.StatusCode;
                var elapsedMs = stopwatch.ElapsedMilliseconds;

                // Chọn Level log dựa trên StatusCode để dễ filter lỗi sau này
                if (statusCode >= 500)
                {
                    Serilog.Log.Error("Gateway Error: {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                        httpMethod, requestPath, statusCode, elapsedMs);
                }
                else if (statusCode >= 400)
                {
                    Serilog.Log.Warning("Gateway Warning: {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                        httpMethod, requestPath, statusCode, elapsedMs);
                }
                else
                {
                    Serilog.Log.Information("Gateway Traffic: {Method} {Path} responded {StatusCode} in {ElapsedMs}ms",
                        httpMethod, requestPath, statusCode, elapsedMs);
                }
            }
        }
    }
}