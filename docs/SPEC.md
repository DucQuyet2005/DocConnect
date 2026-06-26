# ĐẶC TẢ YÊU CẦU PHẦN MỀM (SOFTWARE SPECIFICATION - SPEC)
> **Dự án:** DocConnect - Hệ thống tư vấn sức khỏe trực tuyến
> **Kiến trúc:** Monolithic (Đơn khối)
> **Mô hình giao diện:** ASP.NET Core MVC (Razor Views)

Tài liệu này đặc tả chi tiết các yêu cầu chức năng, luồng nghiệp vụ và mô hình dữ liệu cho dự án **DocConnect**.

---

## 1. Các Vai Trò Người Dùng (Actors & Roles)

Hệ thống phục vụ 3 nhóm đối tượng chính với quyền hạn và giao diện khác nhau:

### 1.1. Bệnh nhân (Patient)
* Đăng ký tài khoản, đăng nhập qua Email/Số điện thoại.
* Cập nhật hồ sơ cá nhân: Họ tên, ngày sinh, số điện thoại, địa chỉ, nhóm máu, tiền sử bệnh án cá nhân.
* Tìm kiếm Bác sĩ theo: Tên bác sĩ, chuyên khoa, mức phí tư vấn, hoặc đánh giá.
* Đặt lịch tư vấn: Chọn ngày, chọn khung giờ trống (Time Slot) của bác sĩ.
* Thanh toán phí tư vấn qua cổng thanh toán trực tuyến.
* Tham gia ca tư vấn: Chat trực tuyến, gọi cuộc gọi video với bác sĩ.
* Xem lịch sử tư vấn, đơn thuốc điện tử được kê bởi bác sĩ.

### 1.2. Bác sĩ (Doctor)
* Đăng ký hồ sơ bác sĩ: Điền chuyên khoa, tiểu sử, số năm kinh nghiệm, mức phí tư vấn, hình ảnh bằng cấp/chứng chỉ hành nghề (chờ Admin duyệt).
* Cập nhật lịch làm việc: Thiết lập các ca trực/khung giờ rảnh theo lịch tuần.
* Quản lý lịch hẹn khám: Chấp nhận, từ chối hoặc hủy lịch hẹn từ bệnh nhân.
* Tiến hành tư vấn trực tuyến: Nhắn tin chat, gọi điện video call cho bệnh nhân khi đến giờ hẹn.
* Ghi chẩn đoán y khoa và kê đơn thuốc điện tử trực tiếp sau ca khám.
* Xem lịch sử hồ sơ bệnh án của bệnh nhân (chỉ những bệnh nhân đã đặt lịch với bác sĩ đó).

### 1.3. Quản trị viên (Admin)
* Đăng nhập trang quản trị dành riêng cho Admin.
* Quản lý người dùng: Khóa/mở khóa tài khoản của bệnh nhân và bác sĩ.
* Phê duyệt hồ sơ bác sĩ: Kiểm tra bằng cấp và chuyển trạng thái tài khoản bác sĩ thành `Verified` (được hiển thị trên trang tìm kiếm).
* Quản lý chuyên khoa: Thêm/Sửa/Xóa các chuyên khoa y tế (Nội khoa, Nhi khoa, Da liễu, Răng Hàm Mặt, v.v.).
* Xem báo cáo & Thống kê: Doanh thu hệ thống, tổng số ca tư vấn, biểu đồ phát triển người dùng.

---

## 2. Luồng Nghiệp Vụ Chi Tiết (Key Workflows)

### 2.1. Luồng Đặt Lịch & Tư Vấn (Booking to Consultation)
1. **Bước 1: Tìm kiếm bác sĩ:** Bệnh nhân xem danh sách bác sĩ chuyên khoa đã được Admin duyệt.
2. **Bước 2: Chọn ca khám:** Bệnh nhân chọn một bác sĩ, xem các khung giờ trống và chọn một ca.
3. **Bước 3: Xác nhận & Thanh toán:** Bệnh nhân điền triệu chứng sơ bộ, chọn phương thức thanh toán và tiến hành thanh toán trực tuyến. Trạng thái lịch hẹn lúc này là `Pending` (Chờ xác nhận).
4. **Bước 4: Bác sĩ nhận lịch:** Lịch hẹn chuyển sang `Confirmed` sau khi thanh toán thành công và bác sĩ chấp nhận ca khám.
5. **Bước 5: Tư vấn Real-time:** Khi tới khung giờ hẹn, phòng tư vấn được mở. Bác sĩ và bệnh nhân tiến hành chat/gọi video trực tiếp.
6. **Bước 6: Hoàn thành & Kê đơn:** Kết thúc thời gian tư vấn, bác sĩ nhập chẩn đoán và đơn thuốc. Lịch hẹn chuyển sang trạng thái `Completed`.

---

## 3. Thiết Kế Cơ Sở Dữ Liệu (Database Schema Design)

Cơ sở dữ liệu sử dụng **SQL Server** và được quản lý thông qua **Entity Framework Core (Code-First)**. Do hệ thống sử dụng kiến trúc Monolith, tất cả các thực thể đều được khai báo trực tiếp trong thư mục `Data/Entities` của dự án `DocConnect.Web`.

### 3.1. Sơ đồ Quan hệ Thực thể (Entity Relationship Overview)
* `User` 1 ➔ 0..1 `Patient`
* `User` 1 ➔ 0..1 `Doctor`
* `Patient` 1 ➔ 0..* `Appointment`
* `Doctor` 1 ➔ 0..* `Appointment`
* `Appointment` 1 ➔ 0..1 `MedicalRecord`
* `Appointment` 1 ➔ 0..* `Message`

### 3.2. Đặc tả các Bảng dữ liệu

#### Bảng `Users` (Người dùng hệ thống)
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Avatar NVARCHAR(500) NULL,
    Role NVARCHAR(20) NOT NULL, -- 'Admin', 'Doctor', 'Patient'
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### Bảng `Patients` (Thông tin Bệnh nhân)
```sql
CREATE TABLE Patients (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL, -- 'Nam', 'Nữ', 'Khác'
    Address NVARCHAR(250) NULL,
    BloodType VARCHAR(5) NULL,    -- 'A', 'B', 'O', 'AB'
    MedicalHistory NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
```

#### Bảng `Doctors` (Thông tin Bác sĩ)
```sql
CREATE TABLE Doctors (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    Specialty NVARCHAR(100) NOT NULL,
    ExperienceYears INT NOT NULL,
    Biography NVARCHAR(MAX) NULL,
    ConsultationFee DECIMAL(18,2) NOT NULL,
    IsVerified BIT NOT NULL DEFAULT 0,
    CertificationUrl NVARCHAR(500) NULL, -- Minh chứng bằng cấp
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
```

#### Bảng `Appointments` (Lịch hẹn tư vấn)
```sql
CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PatientId UNIQUEIDENTIFIER NOT NULL,
    DoctorId UNIQUEIDENTIFIER NOT NULL,
    Date DATE NOT NULL,
    TimeSlot VARCHAR(20) NOT NULL,       -- Ví dụ: '08:00 - 08:30'
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Confirmed', 'Cancelled', 'Completed'
    ConsultationFee DECIMAL(18,2) NOT NULL,
    PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Unpaid', -- 'Unpaid', 'Paid', 'Refunded'
    SymptomsSummary NVARCHAR(500) NULL,  -- Triệu chứng bệnh nhân nhập khi đặt lịch
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
    Prescription NVARCHAR(MAX) NOT NULL,   -- Danh sách thuốc và liều dùng (JSON format)
    Note NVARCHAR(MAX) NULL,               -- Dặn dò thêm
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE
);
```

#### Bảng `Messages` (Tin nhắn tư vấn trực tiếp)
```sql
CREATE TABLE Messages (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AppointmentId UNIQUEIDENTIFIER NOT NULL,
    SenderId UNIQUEIDENTIFIER NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE,
    FOREIGN KEY (SenderId) REFERENCES Users(Id)
);
```
