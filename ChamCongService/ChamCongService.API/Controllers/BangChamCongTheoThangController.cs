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
    public class BangChamCongTheoThangController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BangChamCongTheoThangDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllBangChamCongTheoThangQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BangChamCongTheoThangDto>> GetById(Guid id)
        {
            var result = await mediator.Send(new GetBangChamCongTheoThangByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateBangChamCongTheoThangCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateBangChamCongTheoThangCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã cập nhật bảng chấm công" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteBangChamCongTheoThangCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã xóa bảng chấm công" });
        }

        [HttpPatch("{id}/chot")]
        public async Task<ActionResult> Chot(Guid id)
        {
            var result = await mediator.Send(new ChotBangChamCongCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã chốt bảng chấm công" });
        }

        [HttpPatch("{id}/mo-chot")]
        public async Task<ActionResult> MoChot(Guid id)
        {
            var result = await mediator.Send(new MoChotBangChamCongCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã mở chốt bảng chấm công" });
        }
    }
}
