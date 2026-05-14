using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using QuyetDinhService.QuyetDinhService.Application.Query;
using QuyetDinhService.QuyetDinhService.Application.DTOs.NangLuong;
using QuyetDinhService.QuyetDinhService.Application.Commands;

namespace QuyetDinhService.QuyetDinhService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuyetDinhNangLuongController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuyetDinhNangLuongController(IMediator mediator)
        {
            _mediator = mediator;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuyetDinhNangLuongDto>>> GetQuyetDinhNangLuongs()
        {
            var command = new GetAllQuyetDinhNangLuongQuery();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuyetDinhNangLuongDto>> GetQuyetDinhNangLuong(Guid id)
        {
            var query = new GetQuyetDinhNangLuongByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateQuyetDinhNangLuong([FromBody] CreateQuyetDinhNangLuongCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuyetDinhNangLuong(Guid id, [FromBody] UpdateQuyetDinhNangLuongCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in command");
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuyetDinhNangLuong(Guid id)
        {
            var command = new DeleteQuyetDinhNangLuongCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
