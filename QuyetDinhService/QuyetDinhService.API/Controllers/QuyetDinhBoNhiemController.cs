using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuyetDinhService.QuyetDinhService.Application.Commands;
using QuyetDinhService.QuyetDinhService.Application.DTOs.BoNhiem;
using QuyetDinhService.QuyetDinhService.Application.Query;

namespace QuyetDinhService.QuyetDinhService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuyetDinhBoNhiemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuyetDinhBoNhiemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuyetDinhBoNhiemDto>>> GetQuyetDinhBoNhiems()
        {
            var query = new GetAllQuyetDinhBoNhiemQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuyetDinhBoNhiemDto>> GetQuyetDinhBoNhiem(Guid id)
        {
            var query = new GetQuyetDinhBoNhiemByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateQuyetDinhBoNhiem([FromBody] CreateQuyetDinhBoNhiemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuyetDinhBoNhiem(Guid id, [FromBody] UpdateQuyetDinhBoNhiemCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã cập nhật quyết định bổ nhiệm" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuyetDinhBoNhiem(Guid id)
        {
            var command = new DeleteQuyetDinhBoNhiemCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return Ok(new { message = "Đã xóa quyết định bổ nhiệm" });
        }
    }
}
