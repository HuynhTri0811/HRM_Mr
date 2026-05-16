using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinhLuongService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKyTinhLuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KyTinhLuongs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaKy = table.Column<string>(type: "text", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChotTinhLuong = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyTinhLuongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhanVienTinhLuongs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NhanVienId = table.Column<Guid>(type: "uuid", nullable: false),
                    KyTinhLuongId = table.Column<Guid>(type: "uuid", nullable: false),
                    LuongCoBan = table.Column<decimal>(type: "numeric", nullable: false),
                    PhuCap = table.Column<decimal>(type: "numeric", nullable: false),
                    Thuong = table.Column<decimal>(type: "numeric", nullable: false),
                    Phat = table.Column<decimal>(type: "numeric", nullable: false),
                    TongLuong = table.Column<decimal>(type: "numeric", nullable: false),
                    Thue = table.Column<decimal>(type: "numeric", nullable: false),
                    LuongThucLanh = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVienTinhLuongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhanVienTinhLuongs_KyTinhLuongs_KyTinhLuongId",
                        column: x => x.KyTinhLuongId,
                        principalTable: "KyTinhLuongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhanVienTinhLuongs_KyTinhLuongId",
                table: "NhanVienTinhLuongs",
                column: "KyTinhLuongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhanVienTinhLuongs");

            migrationBuilder.DropTable(
                name: "KyTinhLuongs");
        }
    }
}
