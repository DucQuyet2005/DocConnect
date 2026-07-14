namespace DocConnect.Web.Models.ViewModels
{
    public class BacSiViewModel
    {
        public string Id { get; set; } = string.Empty; // Id người dùng (Bác sĩ)
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string AnhDaiDien { get; set; } = string.Empty;
        public string TenChuyenKhoa { get; set; } = string.Empty;
        public int ChuyenKhoaId { get; set; }
        public string KinhNghiem { get; set; } = string.Empty;
        public string GioiThieu { get; set; } = string.Empty;
        public bool? TrangThai { get; set; }
    }
}