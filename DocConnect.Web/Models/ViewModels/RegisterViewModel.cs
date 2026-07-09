using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public required string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        public required string MatKhau { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public required string NhapLaiMatKhau { get; set; }
        public required string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public required string VaiTro { get; set; }
    }
}