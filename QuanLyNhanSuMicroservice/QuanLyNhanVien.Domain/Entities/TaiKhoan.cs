using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Base;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities
{
    public class TaiKhoan : ObjectBase
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";

        public TaiKhoan()
        {
            Username = string.Empty;
            PasswordHash = string.Empty;
        }

        public TaiKhoan(string username, string passwordHash, string role = "User")
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwordHash))
                throw new Exception("Username và PasswordHash không được để trống");

            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
