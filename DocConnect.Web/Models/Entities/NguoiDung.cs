using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MatKhauHash { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; } // Cho phép null nếu DB cho phép
        public string VaiTro { get; set; } = string.Empty;
        public bool? TrangThai { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public string? AnhDaiDien { get; set; }
    }
}