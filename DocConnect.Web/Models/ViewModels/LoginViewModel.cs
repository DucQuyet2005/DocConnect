using System.ComponentModel.DataAnnotations;

namespace DocConnect.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public required string MatKhau { get; set; }

        public bool GhiNho { get; set; }
    }
}