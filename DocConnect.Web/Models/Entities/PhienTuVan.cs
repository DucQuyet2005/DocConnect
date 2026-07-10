using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
    [Table("PhienTuVan")] // Ép EF Core map chính xác vào bảng PhienTuVan trong SQL
    public class PhienTuVan
    {
        [Key]
        public int Id { get; set; }
        public int LichHenId { get; set; }
        public DateTime? ThoiGianBatDauThucTe { get; set; }
        public DateTime? ThoiGianKetThucThucTe { get; set; }
        public string? KetLuan { get; set; }
        public string? TrangThai { get; set; }
    }
}