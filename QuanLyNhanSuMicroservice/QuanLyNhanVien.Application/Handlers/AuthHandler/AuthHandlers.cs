using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Handlers.AuthHandler
{
    public class LoginHandler(ITaiKhoanRepository repository, IConfiguration configuration) : IRequestHandler<LoginRequestDTO, LoginResponseDTO>
    {
        public async Task<LoginResponseDTO> Handle(LoginRequestDTO request, CancellationToken cancellationToken)
        {
            var user = await repository.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"] ?? "SuperSecretKeyForJwtAuthenticationWhichIsVeryLong!123");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponseDTO(tokenHandler.WriteToken(token));
        }
    }

    public class RegisterHandler(ITaiKhoanRepository repository) : IRequestHandler<RegisterRequestDTO, bool>
    {
        public async Task<bool> Handle(RegisterRequestDTO request, CancellationToken cancellationToken)
        {
            var existingUser = await repository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new Exception("Tên đăng nhập đã tồn tại.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new TaiKhoan(request.Username, passwordHash, request.Role ?? "User");
            
            await repository.AddAsync(newUser);
            return true;
        }
    }
}
