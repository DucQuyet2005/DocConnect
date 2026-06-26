# DocConnect - Hệ thống Tư vấn Sức khỏe Trực tuyến
> **Nền tảng kết nối bệnh nhân và bác sĩ chuyên khoa từ xa.**
> **Công nghệ:** C# / .NET 10.0 (ASP.NET Core MVC) & MS SQL Server.

DocConnect là hệ thống cho phép bệnh nhân tìm kiếm bác sĩ theo chuyên khoa, đặt lịch khám từ xa, thực hiện tư vấn trực tuyến thời gian thực (qua chat và cuộc gọi video), lưu trữ hồ sơ bệnh án và thanh toán trực tuyến thuận tiện.

---

## 🚀 Các Tính Năng Chính

### 1. Phân hệ Bệnh nhân (Patient)
* **Đặt lịch khám:** Tìm kiếm bác sĩ theo chuyên khoa, chọn khung giờ trống và đặt lịch.
* **Tư vấn trực tuyến:** Chat thời gian thực (SignalR) và Gọi cuộc gọi video (WebRTC).
* **Đơn thuốc & Lịch sử khám:** Xem các đơn thuốc điện tử và chẩn đoán y tế từ bác sĩ.
* **Thanh toán trực tuyến:** Thanh toán phí khám bệnh an toàn qua VNPAY/Momo/PayOS.

### 2. Phân hệ Bác sĩ (Doctor)
* **Quản lý lịch rảnh:** Thiết lập các ca trực/khung giờ rảnh theo tuần.
* **Xử lý lịch hẹn:** Phê duyệt, từ chối hoặc hủy ca khám của bệnh nhân.
* **Tư vấn & Kê đơn:** Chat/Gọi video trực tiếp với người bệnh và kê đơn thuốc sau ca khám.

### 3. Phân hệ Quản trị (Admin)
* **Kiểm duyệt bác sĩ:** Xác minh thông tin bằng cấp, chứng chỉ trước khi hiển thị bác sĩ lên hệ thống.
* **Quản lý danh mục:** Quản trị danh mục chuyên khoa và tài khoản người dùng.
* **Báo cáo thống kê:** Theo dõi biểu đồ tăng trưởng người dùng và doanh thu hệ thống.

---

## 🛠️ Công Nghệ Sử Dụng

* **Backend / Giao diện:** ASP.NET Core MVC (.NET 10.0)
* **Cơ sở dữ liệu:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First)
* **Mẫu thiết kế:** Repository Pattern & Unit of Work
* **Real-time (Thời gian thực):** SignalR (Chat/Thông báo) & WebRTC (Peer-to-Peer Video Call)
* **Bảo mật:** Xác thực Cookie, phân quyền RBAC (Role-based access control), và mã hóa đối xứng bệnh án (AES-256).

---

## 📁 Cấu Trúc Dự Án chính

```text
DocConnect/
├── docs/                               # Tài liệu thiết kế & đặc tả hệ thống
│   ├── SPEC.md                         # Đặc tả chi tiết nghiệp vụ và DB Schema
│   └── ARCHITECTURE.md                 # Tài liệu thiết kế kiến trúc và luồng dữ liệu
├── skills/                             # Quy chuẩn và tài liệu kỹ năng lập trình của nhóm
└── DocConnect.Web/                     # Dự án chính ASP.NET Core MVC (Monolith)
    ├── Data/                           # Entities, DbContext, Repositories, UnitOfWork
    ├── Services/                       # Business Logic (AppointmentService, EncryptionService...)
    ├── Controllers/                    # MVC Controllers
    ├── Views/                          # Razor Pages Giao diện
    ├── Hubs/                           # SignalR Hub cho Chat thời gian thực
    └── wwwroot/                        # Tệp tĩnh (CSS, JS, Images, Libraries)
```

---

## ⚙️ Hướng Dẫn Cài Đặt & Chạy Dự Án

### Yêu cầu hệ thống (Prerequisites)
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB hoặc SQL Express)
* Công cụ EF Core CLI (chạy lệnh sau trong terminal để cài đặt):
  ```bash
  dotnet tool install -g dotnet-ef
  ```

### Các bước cài đặt:

1. **Clone dự án về máy:**
   ```bash
   git clone <url-cua-du-an>
   cd DocConnect
   ```

2. **Cấu hình chuỗi kết nối cơ sở dữ liệu:**
   Mở tệp `DocConnect.Web/appsettings.json` và cấu hình lại chuỗi kết nối SQL Server của bạn tại trường `DefaultConnection`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=DocConnectDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Cài đặt thư viện (NuGet packages):**
   Di chuyển vào thư mục dự án Web và khôi phục các thư viện:
   ```bash
   cd DocConnect.Web
   dotnet restore
   ```

4. **Tạo Cơ sở dữ liệu (Database Migration):**
   Sinh cấu trúc bảng dữ liệu SQL Server từ code Entity Framework:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Khởi chạy ứng dụng:**
   Khởi động dự án Web ở chế độ Hot Reload (tự cập nhật khi thay đổi code):
   ```bash
   dotnet watch run
   ```
   Sau khi chạy, ứng dụng sẽ mở cổng mặc định (thường là `https://localhost:5001` hoặc `http://localhost:5000`) trên trình duyệt của bạn.

---

## 📝 Tài Liệu Liên Quan
* [Đặc tả yêu cầu nghiệp vụ (SPEC.md)](file:///e:/User/Work_Space/C%23/BTL/DocConnect/docs/SPEC.md)
* [Kiến trúc và Thiết kế kỹ thuật (ARCHITECTURE.md)](file:///e:/User/Work_Space/C%23/BTL/DocConnect/docs/ARCHITECTURE.md)
