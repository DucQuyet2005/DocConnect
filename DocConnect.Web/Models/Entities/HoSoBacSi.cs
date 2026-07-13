using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? TrangThaiXacThuc { get; set; }

        [ForeignKey("NguoiDungId")]
        public virtual NguoiDung NguoiDung { get; set; } = null!;

        [ForeignKey("ChuyenKhoaId")]
        public virtual ChuyenKhoa ChuyenKhoa { get; set; } = null!;
    }
}