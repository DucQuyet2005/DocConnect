# ĐẶC TẢ YÊU CẦU PHẦN MỀM (SOFTWARE SPECIFICATION - SPEC)
> **Dự án:** DocConnect - Hệ thống tư vấn sức khỏe trực tuyến (Phiên bản tối giản)
> **Kiến trúc:** Monolithic (Đơn khối) áp dụng Repository Pattern & Unit of Work
> **Mô hình giao diện:** ASP.NET Core MVC (Razor Views) kết hợp jQuery AJAX

Tài liệu này đặc tả chi tiết các yêu cầu chức năng cốt lõi, luồng nghiệp vụ và thiết kế cơ sở dữ liệu đã được tối giản hóa cho dự án **DocConnect** phù hợp với quy mô nhóm 3 người.

---

## 1. Các Vai Trò Người Dùng (Actors & Roles)

Hệ thống tập trung hoàn toàn vào hai vai trò chính tương tác trực tiếp với nhau, loại bỏ hoàn toàn vai trò Admin quản trị để tối ưu hóa thời gian phát triển:

### 1.1. Bệnh nhân (Patient)
* **Đăng ký/Đăng nhập:** Đăng ký tài khoản (tab Bệnh nhân) bằng Email, Họ tên, Mật khẩu, Số điện thoại.
* **Cập nhật hồ sơ:** Cập nhật thông tin chi tiết: Ngày sinh, Giới tính, Địa chỉ, Tiền sử bệnh án cá nhân.
* **Tìm kiếm bác sĩ:** Xem danh sách bác sĩ, tìm kiếm theo tên hoặc lọc theo chuyên khoa.
* **Đặt lịch tư vấn:** Chọn bác sĩ, chọn ngày khám và chọn khung giờ trống (Time Slot). Nhập mô tả triệu chứng ban đầu.
* **Phòng tư vấn trực tuyến:** Chat trực tuyến với bác sĩ (gửi và nhận tin nhắn thời gian thực qua cơ chế AJAX polling).
* **Lịch sử khám bệnh:** Xem lại các lịch hẹn đã khám, xem chẩn đoán và đơn thuốc điện tử được kê bởi bác sĩ.

### 1.2. Bác sĩ (Doctor)
* **Đăng ký tài khoản (Tự kích hoạt):** Đăng ký tài khoản (tab Bác sĩ). Chọn chuyên khoa có sẵn hoặc nhập chuyên khoa mới. Nhập số năm kinh nghiệm, tiểu sử ngắn và mức phí tư vấn tham khảo. Tài khoản sau khi đăng ký sẽ tự động hiển thị trên hệ thống (không cần phê duyệt).
* **Quản lý lịch hẹn:** Xem danh sách các yêu cầu đặt lịch hẹn của bệnh nhân. Có quyền "Xác nhận" (Confirm) hoặc "Hủy" (Cancel) lịch hẹn.
* **Tiến hành tư vấn trực tuyến:** Khi đến giờ hẹn, truy cập vào phòng tư vấn để nhắn tin trao đổi trực tiếp với bệnh nhân (qua giao diện chat AJAX).
* **Kê đơn & Chẩn đoán:** Kết thúc buổi tư vấn, nhập chẩn đoán lâm sàng và kê đơn thuốc (lưu thông tin bệnh án). Chuyển trạng thái lịch hẹn thành hoàn thành (`Completed`).
* **Xem hồ sơ bệnh nhân:** Xem hồ sơ y tế và lịch sử khám của bệnh nhân đã đặt lịch với mình.

---

## 2. Luồng Nghiệp Vụ Chi Tiết (Key Workflows)

### 2.1. Đăng ký tài khoản chia 2 Tab
1. Người dùng truy cập trang Đăng ký (`/Account/Register`).
2. Giao diện hiển thị 2 tab: **Đăng ký Bệnh nhân** và **Đăng ký Bác sĩ**.
3. **Nếu chọn tab Bác sĩ:**
   * Hệ thống hiển thị danh sách Chuyên khoa dưới dạng Dropdown (lấy từ bảng `Specialties` đã có sẵn dữ liệu mẫu).
   * Cung cấp một tùy chọn hoặc ô nhập text: "Chuyên khoa khác (nếu không có trong danh sách)".
   * Nếu bác sĩ nhập chuyên khoa mới, khi lưu tài khoản, hệ thống sẽ tự động thêm chuyên khoa đó vào bảng `Specialties` trước khi liên kết với bác sĩ.

### 2.2. Luồng Đặt Lịch & Tư Vấn (Không cần thanh toán)
1. **Bước 1: Tìm kiếm bác sĩ:** Bệnh nhân xem danh sách bác sĩ trên trang chủ hoặc trang tìm kiếm, có thể lọc nhanh theo Chuyên khoa.
2. **Bước 2: Chọn khung giờ:** Bệnh nhân chọn bác sĩ, chọn ngày khám và một khung giờ rảnh (Ví dụ: `08:00 - 08:30`). Nhập tóm tắt triệu chứng.
3. **Bước 3: Gửi yêu cầu:** Bệnh nhân xác nhận đặt lịch. Lịch hẹn được tạo với trạng thái ban đầu là `Pending`.
4. **Bước 4: Bác sĩ duyệt lịch:** Bác sĩ vào danh sách lịch hẹn của mình, nhấn **Xác nhận** (trạng thái chuyển sang `Confirmed`) hoặc **Từ chối/Hủy** (trạng thái chuyển sang `Cancelled`).
5. **Bước 5: Tư vấn Chat AJAX:** Đến giờ hẹn, cả hai bên vào phòng tư vấn (`/Consultation/Room/{appointmentId}`). Hai bên gửi tin nhắn qua lại. Giao diện chat sử dụng JavaScript để tự động gửi request AJAX lấy tin nhắn mới sau mỗi 2 giây (Short Polling) để cập nhật giao diện.
6. **Bước 6: Hoàn thành khám:** Bác sĩ nhấn nút "Kết thúc cuộc hẹn", hệ thống hiển thị form nhập Chẩn đoán và Đơn thuốc. Sau khi lưu, thông tin được lưu vào bảng `MedicalRecords` và trạng thái lịch hẹn chuyển sang `Completed`.

---

## 3. Thiết Kế Cơ Sở Dữ Liệu (Database Schema Design)

Sử dụng **SQL Server** thông qua **Entity Framework Core (Code-First)**. Tất cả các thực thể nằm trong thư mục `Data/Entities` của dự án `DocConnect.Web`.

### 3.1. Sơ đồ Quan hệ Thực thể (Entity Relationship Overview)
* `ApplicationUser` (Kế thừa `IdentityUser`) ➔ `Patient` (1 - 0..1)
* `ApplicationUser` (Kế thừa `IdentityUser`) ➔ `Doctor` (1 - 0..1)
* `Specialty` ➔ `Doctor` (1 - N)
* `Patient` ➔ `Appointment` (1 - N)
* `Doctor` ➔ `Appointment` (1 - N)
* `Appointment` ➔ `MedicalRecord` (1 - 0..1)
* `Appointment` ➔ `Message` (1 - N)

### 3.2. Đặc tả các Bảng dữ liệu

#### Bảng `AspNetUsers` (Được mở rộng thông qua lớp `ApplicationUser`)
```csharp
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public string Role { get; set; } // "Patient" hoặc "Doctor"
    public string? Avatar { get; set; } // Đường dẫn ảnh đại diện
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

#### Bảng `Patients` (Thông tin chi tiết Bệnh nhân)
```sql
CREATE TABLE Patients (
    Id UNIQUEIDENTIFIER PRIMARY KEY, -- Khóa ngoại liên kết tới AspNetUsers(Id)
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,    -- 'Nam', 'Nữ', 'Khác'
    Address NVARCHAR(250) NULL,
    MedicalHistory NVARCHAR(MAX) NULL, -- Tiền sử bệnh án cá nhân
    FOREIGN KEY (Id) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
```

#### Bảng `Specialties` (Chuyên khoa)
```sql
CREATE TABLE Specialties (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE -- Tên chuyên khoa (ví dụ: Nội khoa, Nhi khoa...)
);
```

#### Bảng `Doctors` (Thông tin chi tiết Bác sĩ)
```sql
CREATE TABLE Doctors (
    Id UNIQUEIDENTIFIER PRIMARY KEY, -- Khóa ngoại liên kết tới AspNetUsers(Id)
    SpecialtyId UNIQUEIDENTIFIER NOT NULL,
    ExperienceYears INT NOT NULL,
    Biography NVARCHAR(MAX) NULL,
    ConsultationFee DECIMAL(18,2) NOT NULL,
    CertificationUrl NVARCHAR(500) NULL, -- Đường dẫn tải lên chứng chỉ hành nghề
    FOREIGN KEY (Id) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (SpecialtyId) REFERENCES Specialties(Id)
);
```

#### Bảng `Appointments` (Lịch hẹn khám)
```sql
CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PatientId UNIQUEIDENTIFIER NOT NULL,
    DoctorId UNIQUEIDENTIFIER NOT NULL,
    Date DATE NOT NULL,
    TimeSlot VARCHAR(20) NOT NULL,       -- Ví dụ: '08:00 - 08:30'
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Confirmed', 'Cancelled', 'Completed'
    SymptomsSummary NVARCHAR(500) NULL,  -- Triệu chứng do bệnh nhân nhập khi đặt lịch
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(Id)
);
```

#### Bảng `MedicalRecords` (Hồ sơ bệnh án và đơn thuốc)
```sql
CREATE TABLE MedicalRecords (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AppointmentId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    Symptoms NVARCHAR(MAX) NOT NULL,       -- Triệu chứng ghi nhận bởi bác sĩ
    Diagnosis NVARCHAR(MAX) NOT NULL,      -- Chẩn đoán y khoa
    Treatment NVARCHAR(MAX) NOT NULL,      -- Hướng điều trị
    Prescription NVARCHAR(MAX) NOT NULL,   -- Danh sách thuốc và liều dùng (Text/JSON)
    Note NVARCHAR(MAX) NULL,               -- Lời dặn của bác sĩ
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE
);
```

#### Bảng `Messages` (Lịch sử nhắn tin trong ca tư vấn)
```sql
CREATE TABLE Messages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AppointmentId UNIQUEIDENTIFIER NOT NULL,
    SenderId NVARCHAR(450) NOT NULL,       -- Khóa ngoại liên kết tới AspNetUsers(Id)
    Content NVARCHAR(MAX) NULL,            -- Nội dung văn bản
    MessageType NVARCHAR(10) NOT NULL DEFAULT 'Text', -- 'Text' hoặc 'Image'
    ImageUrl NVARCHAR(500) NULL,           -- Đường dẫn hình ảnh (nếu gửi ảnh)
    SentAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE,
    FOREIGN KEY (SenderId) REFERENCES AspNetUsers(Id)
);
```
