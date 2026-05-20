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
    public class LoaiChamCongController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiChamCongDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllLoaiChamCongQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiChamCongDto>> GetById(Guid id)
        {
            var result = await mediator.Send(new GetLoaiChamCongByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateLoaiChamCongCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateLoaiChamCongCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã cập nhật loại chấm công" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteLoaiChamCongCommand(id));
            if (!result) return NotFound();
            return Ok(new { message = "Đã xóa loại chấm công" });
        }
    }
}
