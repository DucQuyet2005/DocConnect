using Microsoft.AspNetCore.Mvc;
using DocConnect.Web.Models;
using System.Collections.Generic;

namespace DocConnect.Web.Controllers
{
    public class CoSoYTeController : Controller
    {
        // Hàm Index này sẽ chạy khi người dùng truy cập vào đường dẫn /CoSoYTe
        public IActionResult Index(string keyword)
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
                    Id = 13,
                    TenCSYT = "Bệnh viện Chợ Rẫy",
                    LoaiCSYT = "Bệnh viện công",
                    DiaChiShort = "Quận 5, TP. Hồ Chí Minh",
                    DiaChi = "201B Nguyễn Chí Thanh, Quận 5, TP. Hồ Chí Minh",
                    ImageUrl = "/images/csyt/choray.jpg",
                    LogoUrl = "/images/csyt/choray_logo.png"
                },

                new CoSoYTeViewModel
                {
                    Id = 14,
                    TenCSYT = "Bệnh viện Đại học Y Dược TP.HCM",
                    LoaiCSYT = "Bệnh viện công",
                    DiaChiShort = "Quận 5, TP. Hồ Chí Minh",
                    DiaChi = "215 Hồng Bàng, Quận 5, TP. Hồ Chí Minh",
                    ImageUrl = "/images/csyt/ydtphcm.jpg",
                    LogoUrl = "/images/csyt/ydtphcm_logo.png"
                },

                new CoSoYTeViewModel
{
                Id = 15,
                TenCSYT = "Bệnh viện Nhân dân 115",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Quận 10, TP. Hồ Chí Minh",
                DiaChi = "527 Sư Vạn Hạnh, Quận 10, TP. Hồ Chí Minh",
                ImageUrl = "/images/csyt/115.jpg",
                LogoUrl = "/images/csyt/115_logo.png"
            },

                new CoSoYTeViewModel
            {
                Id = 16,
                TenCSYT = "Bệnh viện Gia Định",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Bình Thạnh, TP. Hồ Chí Minh",
                DiaChi = "1 Nơ Trang Long, Bình Thạnh, TP. Hồ Chí Minh",
                ImageUrl = "/images/csyt/giadinh.jpg",
                LogoUrl = "/images/csyt/giadinh_logo.png"
            },
                new CoSoYTeViewModel
            {
                Id = 26,
                TenCSYT = "Bệnh viện Đa khoa Khánh Hòa",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Nha Trang, Khánh Hòa",
                DiaChi = "19 Yersin, Nha Trang",
                ImageUrl = "/images/csyt/khanhhoa.jpg",
                LogoUrl = "/images/csyt/khanhhoa_logo.png"
            },

                new CoSoYTeViewModel
            {
                Id = 27,
                TenCSYT = "Bệnh viện Bãi Cháy",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Hạ Long, Quảng Ninh",
                DiaChi = "Hạ Long, Quảng Ninh",
                ImageUrl = "/images/csyt/baichay.jpg",
                LogoUrl = "/images/csyt/baichay_logo.png"
            },

                new CoSoYTeViewModel
            {
                Id = 28,
                TenCSYT = "Bệnh viện Hữu nghị Đa khoa Nghệ An",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Vinh, Nghệ An",
                DiaChi = "Đại lộ Lê Nin, TP. Vinh",
                ImageUrl = "/images/csyt/nghean.jpg",
                LogoUrl = "/images/csyt/nghean_logo.png"
            },

                new CoSoYTeViewModel
            {
                Id = 29,
                TenCSYT = "Bệnh viện Đa khoa Bình Dương",
                LoaiCSYT = "Bệnh viện công",
                DiaChiShort = "Thủ Dầu Một, Bình Dương",
                DiaChi = "Yersin, Thủ Dầu Một",
                ImageUrl = "/images/csyt/binhduong.jpg",
                LogoUrl = "/images/csyt/binhduong_logo.png"
            }
            };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                danhSachCSYT = danhSachCSYT
                    .Where(x =>
                        x.TenCSYT.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.DiaChi.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.DiaChiShort.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || x.LoaiCSYT.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Keyword = keyword;

            // Truyền danh sách dữ liệu này sang View
            return View(danhSachCSYT);
        }
    }
}