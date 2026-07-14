using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
    [Table("PhienTuVan")]
    public class PhienTuVan
    {
        [Key]
        public int Id { get; set; }
        
        // --- THÊM CÁC TRƯỜNG NÀY VÀO ---
        public string BacSiId { get; set; } = string.Empty; // Người tạo
        public string BenhNhanId { get; set; } = string.Empty; // Người được chỉ định
        // ---------------------------------

        public int LichHenId { get; set; }
        public DateTime? ThoiGianBatDauThucTe { get; set; }
        public DateTime? ThoiGianKetThucThucTe { get; set; }
        public string? KetLuan { get; set; }
        public string? TrangThai { get; set; } // "Active", "Closed"

        // Navigation properties (tùy chọn nhưng rất khuyên dùng)
        [ForeignKey("BacSiId")]
        public NguoiDung? BacSi { get; set; }
        
        [ForeignKey("BenhNhanId")]
        public NguoiDung? BenhNhan { get; set; }

        // --- CÁC TRƯỜNG DÙNG ĐỂ HIỂN THỊ DANH SÁCH INBOX ---
        // Các trường này không cần lưu vào database, chỉ dùng để hiển thị trên view
        [NotMapped] public string? TinNhanCuoi { get; set; }
        [NotMapped] public DateTime? ThoiGianCuoiCung { get; set; }
        [NotMapped] public string? NguoiGuiCuoiId { get; set; }
        [NotMapped] public bool IsUnread { get; set; }
    }
}