using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinhLuongService.Application.Command;
using TinhLuongService.Application.DTOs;
using TinhLuongService.Application.Query;

using Microsoft.AspNetCore.Authorization;

namespace TinhLuongService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KyTinhLuongController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KyTinhLuongDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllKyTinhLuongQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KyTinhLuongDto>> GetById(Guid id)
        {
            var result = await mediator.Send(new GetKyTinhLuongByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateKyTinhLuongCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateKyTinhLuongCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã cập nhật kỳ tính lương" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteKyTinhLuongCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã xóa kỳ tính lương" });
        }

        [HttpPatch("{id}/khoa")]
        public async Task<ActionResult> Khoa(Guid id, [FromQuery] DateTime updatedAt)
        {
            var result = await mediator.Send(new KhoaKyTinhLuongCommand(id, updatedAt));
            if (!result) return NotFound();
            return Ok(new { message = "Đã khóa kỳ tính lương" });
        }

        [HttpPatch("{id}/mo")]
        public async Task<ActionResult> Mo(Guid id, [FromQuery] DateTime updatedAt)
        {
            var result = await mediator.Send(new MoKyTinhLuongCommand(id, updatedAt));
            if (!result) return NotFound();
            return Ok(new { message = "Đã mở kỳ tính lương" });
        }

        [HttpPatch("{id}/tinhluong")]
        public async Task<ActionResult> TinhLuong(Guid id, [FromQuery] DateTime updatedAt)
        {
            var result = await mediator.Send(new TinhLuongCommand(id, updatedAt));
            if (!result) return NotFound();
            return Ok(new { message = "Đã tính lương" });
        }
    }
}
