using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocConnect.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuyenKhoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuyenKhoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenKhoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoSoSucKhoe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhienTuVanId = table.Column<int>(type: "int", nullable: false),
                    TrieuChung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KetLuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenBacSi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoSucKhoe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LichHen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BacSiId = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichHen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoiDap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tuoi = table.Column<int>(type: "int", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChuyenKhoaId = table.Column<int>(type: "int", nullable: true),
                    NguoiDungId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    AnDanh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiDap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoiDap_NguoiDung_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HoSoBacSi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChuyenKhoaId = table.Column<int>(type: "int", nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiThieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuongDanChungChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiXacThuc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoBacSi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoSoBacSi_ChuyenKhoa_ChuyenKhoaId",
                        column: x => x.ChuyenKhoaId,
                        principalTable: "ChuyenKhoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoSoBacSi_NguoiDung_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhienTuVan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BacSiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BenhNhanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LichHenId = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBatDauThucTe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianKetThucThucTe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KetLuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhienTuVan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhienTuVan_NguoiDung_BacSiId",
                        column: x => x.BacSiId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhienTuVan_NguoiDung_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TinNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhienTuVanId = table.Column<int>(type: "int", nullable: false),
                    NguoiGuiId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiTinNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinNhan_PhienTuVan_PhienTuVanId",
                        column: x => x.PhienTuVanId,
                        principalTable: "PhienTuVan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiDap_NguoiDungId",
                table: "HoiDap",
                column: "NguoiDungId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBacSi_ChuyenKhoaId",
                table: "HoSoBacSi",
                column: "ChuyenKhoaId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBacSi_NguoiDungId",
                table: "HoSoBacSi",
                column: "NguoiDungId");

            migrationBuilder.CreateIndex(
                name: "IX_PhienTuVan_BacSiId",
                table: "PhienTuVan",
                column: "BacSiId");

            migrationBuilder.CreateIndex(
                name: "IX_PhienTuVan_BenhNhanId",
                table: "PhienTuVan",
                column: "BenhNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhan_PhienTuVanId",
                table: "TinNhan",
                column: "PhienTuVanId");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[GetChiTietBacSi]
    @BacSiId NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm khám chữa bệnh.') AS KinhNghiem,
        ISNULL(h.GioiThieu, N'Đang cập nhật thông tin giới thiệu.') AS GioiThieu
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor' AND n.Id = @BacSiId;
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[GetDanhSachBacSi]
    @ChuyenKhoaId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm') AS KinhNghiem,
        ISNULL(h.GioiThieu, '') AS GioiThieu
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor'
      AND (
            n.TrangThai = '1' 
            OR n.TrangThai = 'true' 
            OR TRY_CAST(n.TrangThai AS NVARCHAR) = N'Hoạt động'
            OR n.TrangThai IS NULL
          )
      AND (@ChuyenKhoaId IS NULL OR @ChuyenKhoaId = 0 OR h.ChuyenKhoaId = @ChuyenKhoaId);
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE [dbo].[GetTop5BacSi]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm') AS KinhNghiem,
        ISNULL(h.GioiThieu, '') AS GioiThieu
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor'
      AND n.TrangThai = 1
    ORDER BY n.NgayTao DESC; 
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetChiTietBacSi]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetDanhSachBacSi]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetTop5BacSi]");

            migrationBuilder.DropTable(
                name: "HoiDap");

            migrationBuilder.DropTable(
                name: "HoSoBacSi");

            migrationBuilder.DropTable(
                name: "HoSoSucKhoe");

            migrationBuilder.DropTable(
                name: "LichHen");

            migrationBuilder.DropTable(
                name: "TinNhan");

            migrationBuilder.DropTable(
                name: "ChuyenKhoa");

            migrationBuilder.DropTable(
                name: "PhienTuVan");

            migrationBuilder.DropTable(
                name: "NguoiDung");
        }
    }
}
