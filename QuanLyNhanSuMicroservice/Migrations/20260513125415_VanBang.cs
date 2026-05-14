using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhanSuMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class VanBang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VanBangs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenVanBang = table.Column<string>(type: "text", nullable: false),
                    LoaiVanBang = table.Column<string>(type: "text", nullable: false),
                    NgayCap = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NoiCap = table.Column<string>(type: "text", nullable: false),
                    NhanVienID = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_VanBangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VanBangs_NhanViens_NhanVienID",
                        column: x => x.NhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VanBangs_NhanVienID",
                table: "VanBangs",
                column: "NhanVienID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VanBangs");
        }
    }
}
