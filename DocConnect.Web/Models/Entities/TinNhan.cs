using System;
using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.Entities
{
    public class TinNhan
    {
        [Key]
        public int Id { get; set; }
        public int PhienTuVanId { get; set; }
        public string NguoiGuiId { get; set; } = string.Empty;
        public string? NoiDung { get; set; }
        public string? LoaiTinNhan { get; set; }
        public DateTime ThoiGianGui { get; set; }
    }
}