using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
    [Table("TinNhan")] // Ép EF Core map chính xác vào bảng TinNhan trong SQL
    public class TinNhan
    {
        [Key]
        public int Id { get; set; }
       public int PhienTuVanId { get; set; } // Khóa ngoại
        [ForeignKey("PhienTuVanId")]
        public PhienTuVan PhienTuVan { get; set; } // Thuộc tính điều hướng (Navigation)
        public string NguoiGuiId { get; set; } = string.Empty;
        public string? NoiDung { get; set; }
        public string? LoaiTinNhan { get; set; }
        public DateTime ThoiGianGui { get; set; }
    }
}