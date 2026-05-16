using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChamCongService.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhieuChamCongToPhieuDangKyNghi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuDangKyNghis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NgayNghi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LyDo = table.Column<string>(type: "text", nullable: false),
                    MaNhanVien = table.Column<Guid>(type: "uuid", nullable: false),
                    LoaiChamCongId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    LoaiBuoi = table.Column<int>(type: "integer", nullable: true),
                    TuGio = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DenGio = table.Column<TimeSpan>(type: "interval", nullable: true),
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
                    table.PrimaryKey("PK_PhieuDangKyNghis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhieuDangKyNghis_LoaiChamCongs_LoaiChamCongId",
                        column: x => x.LoaiChamCongId,
                        principalTable: "LoaiChamCongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangKyNghis_LoaiChamCongId",
                table: "PhieuDangKyNghis",
                column: "LoaiChamCongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuDangKyNghis");
        }
    }
}
