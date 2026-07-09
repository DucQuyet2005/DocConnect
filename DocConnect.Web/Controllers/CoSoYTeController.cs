using Microsoft.AspNetCore.Mvc;
using DocConnect.Web.Models;
using System.Collections.Generic;

namespace DocConnect.Web.Controllers
{
    public class CoSoYTeController : Controller
    {
        // Hàm Index này sẽ chạy khi người dùng truy cập vào đường dẫn /CoSoYTe
        public IActionResult Index()
        {
            // Mock Data (Dữ liệu giả lập chạy thử giống hệt trong ảnh thiết kế của bạn)
            var danhSachCSYT = new List<CoSoYTeViewModel>
            {
                new CoSoYTeViewModel
                {
                    Id = 1,
                    TenCSYT = "Bệnh Viện 19-8 Bộ Công An",
                    LoaiCSYT = "Bệnh viện công",
                    DiaChiShort = "Mai Dịch, Cầu Giấy, Hà Nội",
                    DiaChi = "Số 9 đường Trần Bình, Phường Mai Dịch, Quận Cầu Giấy, Hà Nội",
                    ImageUrl = "/images/csyt/bv198_bg.jpg", // Bạn chuẩn bị ảnh bỏ vào wwroot
                    LogoUrl = "/images/csyt/bv198_logo.png"
                },
                new CoSoYTeViewModel
                {
                    Id = 2,
                    TenCSYT = "Tổ Hợp Phòng Khám MEDIPLUS",
                    LoaiCSYT = "Phòng khám đa khoa",
                    DiaChiShort = "Hoàng Mai, Hà Nội",
                    DiaChi = "Tầng 2, Trung tâm Thương mại Mandala, Đường Tân Mai, Hoàng Mai, Hà Nội",
                    ImageUrl = "/images/csyt/mediplus_bg.jpg",
                    LogoUrl = "/images/csyt/mediplus_logo.png"
                },
                new CoSoYTeViewModel
                {
                    Id = 3,
                    TenCSYT = "Trung Tâm Chăm Sóc Chân Trời Mới",
                    LoaiCSYT = "Trung tâm Y tế",
                    DiaChiShort = "Thanh Xuân, Hà Nội",
                    DiaChi = "Số 15 Khuất Duy Tiến, Thanh Xuân, Hà Nội",
                    ImageUrl = "/images/csyt/chamtromoi_bg.jpg",
                    LogoUrl = "/images/csyt/chantroimoi_logo.png"
                }
            };

            // Truyền danh sách dữ liệu này sang View
            return View(danhSachCSYT);
        }
    }
}