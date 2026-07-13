using System;
using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.Entities
{
    public class LichHen
    {
        [Key]
        public int Id { get; set; }
        public string NguoiDungId { get; set; } = string.Empty;
        public int BacSiId { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public string? TrangThai { get; set; }
    }
}