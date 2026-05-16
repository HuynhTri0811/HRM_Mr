using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.PhongBan;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhongBanController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllPhongBanQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePhongBanDTO dto)
        {
            var result = await mediator.Send(dto);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePhongBanDTo(id);
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePhongBanRequestDTO dto)
        {
            var command = new UpdatePhongBanDTO(id, dto.MaPhongBan, dto.TenPhongBan);
            var result = await mediator.Send(command);
            return Ok(result);
        }

    }
}
