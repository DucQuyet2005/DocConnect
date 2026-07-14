using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocConnect.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiToStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Update GetChiTietBacSi
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[GetChiTietBacSi]
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
                        ISNULL(h.GioiThieu, N'Đang cập nhật thông tin giới thiệu.') AS GioiThieu,
                        n.TrangThai
                    FROM HoSoBacSi h
                    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
                    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
                    WHERE n.VaiTro = 'Doctor' AND n.Id = @BacSiId;
                END
            ");

            // 2. Update GetDanhSachBacSi
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
                        ISNULL(h.GioiThieu, '') AS GioiThieu,
                        n.TrangThai
                    FROM HoSoBacSi h
                    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
                    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
                    WHERE n.VaiTro = 'Doctor'
                      AND (@ChuyenKhoaId IS NULL OR @ChuyenKhoaId = 0 OR h.ChuyenKhoaId = @ChuyenKhoaId)
                      AND (@Keyword IS NULL OR @Keyword = '' OR n.HoTen LIKE '%' + @Keyword + '%' OR c.TenChuyenKhoa LIKE '%' + @Keyword + '%');
                END
            ");

            // 3. Update GetTop5BacSi
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[GetTop5BacSi]
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
                        ISNULL(h.GioiThieu, '') AS GioiThieu,
                        n.TrangThai
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
            // 1. Revert GetChiTietBacSi
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[GetChiTietBacSi]
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

            // 2. Revert GetDanhSachBacSi
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
                      AND (@ChuyenKhoaId IS NULL OR @ChuyenKhoaId = 0 OR h.ChuyenKhoaId = @ChuyenKhoaId)
                      AND (@Keyword IS NULL OR @Keyword = '' OR n.HoTen LIKE '%' + @Keyword + '%' OR c.TenChuyenKhoa LIKE '%' + @Keyword + '%');
                END
            ");

            // 3. Revert GetTop5BacSi
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[GetTop5BacSi]
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
    }
}
