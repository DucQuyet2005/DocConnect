using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace DocConnect.Web.Models.Entities
{
    [Table("HoSoBacSi")]
    [Index(nameof(AccessCode), IsUnique = true)]
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



        [Required(ErrorMessage = "Bat buoc nhap ma truy xuat")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Ma truy xuat phai co do dai 10 ky tu")]
        [RegularExpression(@"^[A-Z]{2}.*[0-9]$", ErrorMessage = "Ma truy xuat phai bat dau bang 2 chu cai in hoa va ket thuc bang 1 chu so")]
        [Display(Name = "Mã truy xuất")]


        public string AccessCode { get; set; } = string.Empty;

        [ForeignKey("NguoiDungId")]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public virtual NguoiDung NguoiDung { get; set; } = null!;

        [ForeignKey("ChuyenKhoaId")]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public virtual ChuyenKhoa ChuyenKhoa { get; set; } = null!;
    }
}