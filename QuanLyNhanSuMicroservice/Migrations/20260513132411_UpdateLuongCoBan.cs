using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhanSuMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLuongCoBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongLuong",
                table: "NhanViens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TongLuong",
                table: "NhanViens",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
