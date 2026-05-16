using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.Auth;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var result = await mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            try
            {
                var result = await mediator.Send(request);
                return Ok(new { message = "Đăng ký thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
