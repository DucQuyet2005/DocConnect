using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DocConnect.Web.Models.ViewModels
{
    public class HoiDapViewModel
    {
        public string? TieuDe { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung câu hỏi")]
        public required string NoiDung { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tuổi")]
        public int Tuoi { get; set; }

        public string? GioiTinh { get; set; }

        // Nhận file ảnh từ trình duyệt truyền lên
        public IFormFile? FileAnh { get; set; }

        public int? ChuyenKhoaId { get; set; }
    }
}