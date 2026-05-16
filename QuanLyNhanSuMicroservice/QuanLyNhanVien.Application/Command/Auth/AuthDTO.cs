using MediatR;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.Auth
{
    public record LoginRequestDTO(string Username, string Password) : IRequest<LoginResponseDTO>;
    public record LoginResponseDTO(string Token);
    public record RegisterRequestDTO(string Username, string Password, string Role) : IRequest<bool>;
}
