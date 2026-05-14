using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuyetDinhService.Migrations
{
    /// <inheritdoc />
    public partial class AddQuyetDinhNangLuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuyetDinh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoQuyetDinh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    NhanVienId = table.Column<int>(type: "integer", nullable: false),
                    LoaiQuyetDinh = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NgayHieuLuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinh", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhNangLuong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NhanVienId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LuongCu = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LuongMoi = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhNangLuong", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyetDinh");

            migrationBuilder.DropTable(
                name: "QuyetDinhNangLuong");
        }
    }
}
