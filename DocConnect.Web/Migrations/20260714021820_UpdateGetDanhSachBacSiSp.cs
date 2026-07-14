using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocConnect.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetDanhSachBacSiSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HoiDap_ChuyenKhoaId",
                table: "HoiDap",
                column: "ChuyenKhoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiDap_ChuyenKhoa_ChuyenKhoaId",
                table: "HoiDap",
                column: "ChuyenKhoaId",
                principalTable: "ChuyenKhoa",
                principalColumn: "Id");

            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetDanhSachBacSi]
    @ChuyenKhoaId INT = NULL,
    @Keyword NVARCHAR(100) = NULL
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
      AND (@ChuyenKhoaId IS NULL OR @ChuyenKhoaId = 0 OR h.ChuyenKhoaId = @ChuyenKhoaId)
      AND (@Keyword IS NULL OR @Keyword = '' OR n.HoTen LIKE '%' + @Keyword + '%' OR c.TenChuyenKhoa LIKE '%' + @Keyword + '%');
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[GetDanhSachBacSi]
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

            migrationBuilder.DropForeignKey(
                name: "FK_HoiDap_ChuyenKhoa_ChuyenKhoaId",
                table: "HoiDap");

            migrationBuilder.DropIndex(
                name: "IX_HoiDap_ChuyenKhoaId",
                table: "HoiDap");
        }
    }
}
