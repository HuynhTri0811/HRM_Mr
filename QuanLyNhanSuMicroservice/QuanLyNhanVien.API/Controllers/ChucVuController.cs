using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.ChucVu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChucVuController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllChucVuQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetChucVuByIdQuery(id);
            var result = await mediator.Send(query);
            if (result == null) return NotFound("Không tìm thấy chức vụ");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChucVuDTO dto)
        {
            var result = await mediator.Send(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChucVuRequestDTO dto)
        {
            var command = new UpdateChucVuDTO(id, dto.MaChucVu, dto.TenChucVu, dto.PhuCap);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteChucVuDTO(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}
