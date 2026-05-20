using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChamCongService.Application.Commands;
using ChamCongService.Application.DTOs;
using ChamCongService.Application.Query;
using Microsoft.AspNetCore.Authorization;

namespace ChamCongService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhieuDangKyNghiController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuDangKyNghiDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllPhieuDangKyNghiQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuDangKyNghiDto>> GetById(Guid id)
        {
            var result = await mediator.Send(new GetPhieuDangKyNghiByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePhieuDangKyNghiCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdatePhieuDangKyNghiCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã cập nhật phiếu đăng ký nghỉ" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeletePhieuDangKyNghiCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã xóa phiếu đăng ký nghỉ" });
        }
    }
}
