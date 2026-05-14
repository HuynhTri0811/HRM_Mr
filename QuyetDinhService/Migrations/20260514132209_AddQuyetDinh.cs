using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuyetDinhService.Migrations
{
    /// <inheritdoc />
    public partial class AddQuyetDinh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyetDinh");

            migrationBuilder.DropTable(
                name: "QuyetDinhNangLuong");

            migrationBuilder.CreateTable(
                name: "QuyetDinhBoNhiems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaNhanVien = table.Column<Guid>(type: "uuid", nullable: false),
                    ChucVuCu = table.Column<Guid>(type: "uuid", nullable: false),
                    ChucVuMoi = table.Column<Guid>(type: "uuid", nullable: false),
                    PhuCapCu = table.Column<decimal>(type: "numeric", nullable: false),
                    PhuCapMoi = table.Column<decimal>(type: "numeric", nullable: false),
                    LyDo = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    SoQuyetDinh = table.Column<string>(type: "text", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    NgayHieuLuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhBoNhiems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhNangLuongs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaNhanVien = table.Column<Guid>(type: "uuid", nullable: false),
                    LuongCoBanCu = table.Column<decimal>(type: "numeric", nullable: false),
                    LuongCoBanMoi = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    SoQuyetDinh = table.Column<string>(type: "text", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    NgayHieuLuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GhiChu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhNangLuongs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyetDinhBoNhiems");

            migrationBuilder.DropTable(
                name: "QuyetDinhNangLuongs");

            migrationBuilder.CreateTable(
                name: "QuyetDinh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    LoaiQuyetDinh = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NgayHieuLuc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NhanVienId = table.Column<int>(type: "integer", nullable: false),
                    NoiDung = table.Column<string>(type: "text", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    GhiChu = table.Column<string>(type: "text", nullable: true),
                    LuongCu = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LuongMoi = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    NgayQuyetDinh = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NhanVienId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoQuyetDinh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhNangLuong", x => x.Id);
                });
        }
    }
}
