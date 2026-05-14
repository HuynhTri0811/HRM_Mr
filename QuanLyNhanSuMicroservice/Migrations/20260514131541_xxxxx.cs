using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhanSuMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class xxxxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_ChucVus_ChucVuID",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_PhongBans_PhongBanID",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_VanBangs_NhanViens_NhanVienID",
                table: "VanBangs");

            migrationBuilder.RenameColumn(
                name: "NhanVienID",
                table: "VanBangs",
                newName: "NhanVienId");

            migrationBuilder.RenameIndex(
                name: "IX_VanBangs_NhanVienID",
                table: "VanBangs",
                newName: "IX_VanBangs_NhanVienId");

            migrationBuilder.RenameColumn(
                name: "PhongBanID",
                table: "NhanViens",
                newName: "PhongBanId");

            migrationBuilder.RenameColumn(
                name: "ChucVuID",
                table: "NhanViens",
                newName: "ChucVuId");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_PhongBanID",
                table: "NhanViens",
                newName: "IX_NhanViens_PhongBanId");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_ChucVuID",
                table: "NhanViens",
                newName: "IX_NhanViens_ChucVuId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChucVuId",
                table: "NhanViens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_ChucVus_ChucVuId",
                table: "NhanViens",
                column: "ChucVuId",
                principalTable: "ChucVus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_PhongBans_PhongBanId",
                table: "NhanViens",
                column: "PhongBanId",
                principalTable: "PhongBans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VanBangs_NhanViens_NhanVienId",
                table: "VanBangs",
                column: "NhanVienId",
                principalTable: "NhanViens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_ChucVus_ChucVuId",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_PhongBans_PhongBanId",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_VanBangs_NhanViens_NhanVienId",
                table: "VanBangs");

            migrationBuilder.RenameColumn(
                name: "NhanVienId",
                table: "VanBangs",
                newName: "NhanVienID");

            migrationBuilder.RenameIndex(
                name: "IX_VanBangs_NhanVienId",
                table: "VanBangs",
                newName: "IX_VanBangs_NhanVienID");

            migrationBuilder.RenameColumn(
                name: "PhongBanId",
                table: "NhanViens",
                newName: "PhongBanID");

            migrationBuilder.RenameColumn(
                name: "ChucVuId",
                table: "NhanViens",
                newName: "ChucVuID");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_PhongBanId",
                table: "NhanViens",
                newName: "IX_NhanViens_PhongBanID");

            migrationBuilder.RenameIndex(
                name: "IX_NhanViens_ChucVuId",
                table: "NhanViens",
                newName: "IX_NhanViens_ChucVuID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChucVuID",
                table: "NhanViens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_ChucVus_ChucVuID",
                table: "NhanViens",
                column: "ChucVuID",
                principalTable: "ChucVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NhanViens_PhongBans_PhongBanID",
                table: "NhanViens",
                column: "PhongBanID",
                principalTable: "PhongBans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VanBangs_NhanViens_NhanVienID",
                table: "VanBangs",
                column: "NhanVienID",
                principalTable: "NhanViens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
