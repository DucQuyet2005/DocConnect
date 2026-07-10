using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocConnect.Models.Entities; // Hoặc namespace chứa NguoiDung tùy dự án của bạn

namespace DocConnect.Web.Models.Entities
{
    [Table("HoSoBacSi")]
    public class HoSoBacSi
    {
        [Key]
        public int Id { get; set; }

        public string NguoiDungId { get; set; } = string.Empty;
        public int ChuyenKhoaId { get; set; }

        public string? KinhNghiem { get; set; }
        public string? GioiThieu { get; set; }
        public string? DuongDanChungChi { get; set; }

        // === ĐÃ SỬA LỖI: Chuyển đổi từ bool sang string? để khớp với SQL Server ===
        public string? TrangThaiXacThuc { get; set; }

        // --- KHAI BÁO LIÊN KẾT ---
        [ForeignKey("NguoiDungId")]
        public virtual NguoiDung NguoiDung { get; set; } = null!;

        [ForeignKey("ChuyenKhoaId")]
        public virtual ChuyenKhoa ChuyenKhoa { get; set; } = null!;
    }
}