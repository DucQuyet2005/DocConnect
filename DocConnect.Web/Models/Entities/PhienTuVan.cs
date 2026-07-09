using System;
using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.Entities
{
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