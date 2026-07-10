using Microsoft.EntityFrameworkCore;
using DocConnect.Models.Entities;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models;

namespace DocConnect.Web.Data
{
    public class DocConnectDbContext : DbContext
    {
        public DocConnectDbContext(DbContextOptions<DocConnectDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<HoiDap> HoiDaps { get; set; }
        public DbSet<HoSoBacSi> HoSoBacSis { get; set; }
        public DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }
        public DbSet<LichHen> LichHens { get; set; }
        public DbSet<PhienTuVan> PhienTuVans { get; set; }
        public DbSet<TinNhan> TinNhans { get; set; }
        public DbSet<HoSoSucKhoe> HoSoSucKhoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ép buộc EF Core giữ nguyên tên bảng số ít khớp chính xác database
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
            modelBuilder.Entity<HoiDap>().ToTable("HoiDap");
            modelBuilder.Entity<HoSoBacSi>().ToTable("HoSoBacSi");
            modelBuilder.Entity<ChuyenKhoa>().ToTable("ChuyenKhoa");
            modelBuilder.Entity<LichHen>().ToTable("LichHen");
            modelBuilder.Entity<PhienTuVan>().ToTable("PhienTuVan");
            modelBuilder.Entity<TinNhan>().ToTable("TinNhan");
            modelBuilder.Entity<HoSoSucKhoe>().ToTable("HoSoSucKhoe");
        }
    }
}