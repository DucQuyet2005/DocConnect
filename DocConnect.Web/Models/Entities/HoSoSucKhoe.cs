using System;
using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.Entities
{
    public class HoSoSucKhoe
    {
        [Key]
        public int Id { get; set; }
        public string NguoiDungId { get; set; } = string.Empty;
        public int PhienTuVanId { get; set; }
        public string? TrieuChung { get; set; }
        public string? KetLuan { get; set; }
        public string? TenBacSi { get; set; }
        public DateTime ThoiGian { get; set; }
    }
}