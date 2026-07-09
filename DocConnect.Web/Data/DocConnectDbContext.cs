using Microsoft.EntityFrameworkCore;
using DocConnect.Models.Entities;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models;

namespace DocConnect.Web.Data
{
    public class DocConnectDbContext : DbContext
    {
        public DocConnectDbContext(DbContextOptions<DocConnectDbContext> options) : base(options) { }

        // 1. Các bảng cốt lõi bạn đã khai báo
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<HoiDap> HoiDaps { get; set; }

        public DbSet<HoSoBacSi> HoSoBacSis { get; set; }
        public DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }
        public DbSet<LichHen> LichHens { get; set; }
        public DbSet<PhienTuVan> PhienTuVans { get; set; }
        public DbSet<TinNhan> TinNhans { get; set; }
        public DbSet<HoSoSucKhoe> HoSoSucKhoes { get; set; }

    }

}