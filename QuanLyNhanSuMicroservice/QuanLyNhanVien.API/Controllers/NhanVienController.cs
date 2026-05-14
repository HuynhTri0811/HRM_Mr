using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.NhanSu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NhanVienController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllEmployeesQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetEmployeeByIdQuery(id);
            var result = await mediator.Send(query);
            if (result == null) return NotFound("Không tìm thấy nhân viên");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNhanVienDto dto)
        {
            var result = await mediator.Send(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNhanVienRequestDTO dto)
        {
            var command = new UpdateNhanVienDTO(
                id,
                dto.MaNhanVien,
                dto.TenNhanVien,
                dto.NgaySinh,
                dto.GioiTinh,
                dto.Email,
                dto.PhongBanID,
                dto.ChucVuID
            );
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteNhanVienDTO(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}/update-luong")]
        public async Task<IActionResult> UpdateLuong(Guid id, [FromBody] UpdateNhanVienCapNhatLuongRequestDTO dto)
        {
            var command = new UpdateNhanVienCapNhatLuongDTO(id, dto.LuongCoBan);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}/update-chuc-vu")]
        public async Task<IActionResult> UpdateChucVu(Guid id, [FromBody] UpdateNhanVienCapNhatChucVuRequestDTO dto)
        {
            var command = new UpdateNhanVienCapNhatChucVuDTO(id, dto.ChucVuID);
            var result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
