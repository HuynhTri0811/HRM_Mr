using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TinhLuongService.API.Filters
{
    public class ConcurrencyExceptionFilter(ILogger<ConcurrencyExceptionFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DbUpdateConcurrencyException)
            {
                logger.LogWarning(context.Exception, "Xung đột concurrency (bất đồng hành) xảy ra khi cập nhật thực thể.");
                
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Database Concurrency Conflict",
                    Detail = "Dữ liệu đã bị thay đổi bởi người dùng khác kể từ khi bạn tải trang. Vui lòng tải lại trang và thực hiện lại thao tác."
                };

                context.Result = new ConflictObjectResult(problemDetails);
                context.ExceptionHandled = true;
            }
        }
    }
}
