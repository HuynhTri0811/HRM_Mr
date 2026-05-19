using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace QuyetDinhService.API.Filters
{
    public class ResponseLoggingFilter(ILogger<ResponseLoggingFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;
            var path = request.Path;

            // Thực thi action chính
            var resultContext = await next();

            // Intercept kết quả sau khi thực thi thành công
            if (resultContext.Exception == null)
            {
                if (resultContext.Result is ObjectResult objectResult)
                {
                    logger.LogInformation("QuyetDinhService Trả về dữ liệu thành công! [{Method}] {Path} - Trả về: {@ResponseData}", 
                        method, path, objectResult.Value);
                }
                else if (resultContext.Result is StatusCodeResult statusCodeResult)
                {
                    logger.LogInformation("QuyetDinhService Trả về mã trạng thái thành công [{Method}] {Path} - Trả về mã trạng thái: {StatusCode}", 
                        method, path, statusCodeResult.StatusCode);
                }
                else if (resultContext.Result is NoContentResult)
                {
                    logger.LogInformation("QuyetDinhService [{Method}] {Path} - Trả về NoContent", 
                        method, path);
                }
            }
            else
            {
                logger.LogError(resultContext.Exception, "QuyetDinhService [{Method}] {Path} - Gặp lỗi trong quá trình xử lý", 
                    method, path);
            }
        }
    }
}
