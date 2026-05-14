using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.VanBang;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VanBangController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await mediator.Send(new GetAllVanBangQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetVanBangByIdQuery(id));
            if (result == null) return NotFound("Không tìm thấy văn bằng");
            return Ok(result);
        }

        [HttpGet("nhanvien/{nhanVienId}")]
        public async Task<IActionResult> GetByNhanVien(Guid nhanVienId)
        {
            return Ok(await mediator.Send(new GetVanBangByNhanVienQuery(nhanVienId)));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVanBangDto dto)
        {
            return Ok(await mediator.Send(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVanBangRequestDTO dto)
        {
            var command = new UpdateVanBangDTO(id, dto.TenVanBang, dto.LoaiVanBang, dto.NgayCap, dto.NoiCap, dto.NhanVienID);
            return Ok(await mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await mediator.Send(new DeleteVanBangDTO(id)));
        }
    }
}
