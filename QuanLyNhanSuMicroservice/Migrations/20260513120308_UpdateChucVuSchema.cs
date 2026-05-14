using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhanSuMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChucVuSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChucVus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaChucVu = table.Column<string>(type: "text", nullable: false),
                    TenChucVu = table.Column<string>(type: "text", nullable: false),
                    PhuCap = table.Column<decimal>(type: "numeric", nullable: false),
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
                    table.PrimaryKey("PK_ChucVus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhongBans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaQuanLy = table.Column<string>(type: "text", nullable: false),
                    TenPhongBan = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_PhongBans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaNhanVien = table.Column<string>(type: "text", nullable: false),
                    TenNhanVien = table.Column<string>(type: "text", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GioiTinh = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TongLuong = table.Column<decimal>(type: "numeric", nullable: false),
                    LuongCoBan = table.Column<decimal>(type: "numeric", nullable: false),
                    PhuCap = table.Column<decimal>(type: "numeric", nullable: false),
                    ChucVuID = table.Column<Guid>(type: "uuid", nullable: false),
                    PhongBanID = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_NhanViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhanViens_ChucVus_ChucVuID",
                        column: x => x.ChucVuID,
                        principalTable: "ChucVus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NhanViens_PhongBans_PhongBanID",
                        column: x => x.PhongBanID,
                        principalTable: "PhongBans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_ChucVuID",
                table: "NhanViens",
                column: "ChucVuID");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_PhongBanID",
                table: "NhanViens",
                column: "PhongBanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "ChucVus");

            migrationBuilder.DropTable(
                name: "PhongBans");
        }
    }
}
