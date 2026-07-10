using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
    [Table("HoiDap")]
    public class HoiDap
    {
        [Key]
        public int Id { get; set; }

        public string? TieuDe { get; set; }

        [Required]
        public required string NoiDung { get; set; }

        [Required]
        public int Tuoi { get; set; }

        public string? GioiTinh { get; set; }

        public string? DuongDanAnh { get; set; }

        public int? ChuyenKhoaId { get; set; }

        public string? NguoiDungId { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public bool DaDuyet { get; set; } = false;

        public bool AnDanh { get; set; } = true;

        [ForeignKey("NguoiDungId")]
        public virtual NguoiDung? NguoiDung { get; set; } // Đã sửa từ DocConnectDbContext thành NguoiDung
    }
}