USE [master]
GO
/****** Object:  Database [DocConnect]    Script Date: 7/10/2026 7:08:31 AM ******/
CREATE DATABASE [DocConnect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DocConnect', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DocConnect.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DocConnect_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DocConnect_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DocConnect] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DocConnect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DocConnect] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DocConnect] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DocConnect] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DocConnect] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DocConnect] SET ARITHABORT OFF 
GO
ALTER DATABASE [DocConnect] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DocConnect] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DocConnect] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DocConnect] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DocConnect] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DocConnect] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DocConnect] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DocConnect] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DocConnect] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DocConnect] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DocConnect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DocConnect] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DocConnect] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DocConnect] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DocConnect] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DocConnect] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DocConnect] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DocConnect] SET RECOVERY FULL 
GO
ALTER DATABASE [DocConnect] SET  MULTI_USER 
GO
ALTER DATABASE [DocConnect] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DocConnect] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DocConnect] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DocConnect] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DocConnect] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DocConnect] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DocConnect', N'ON'
GO
ALTER DATABASE [DocConnect] SET QUERY_STORE = ON
GO
ALTER DATABASE [DocConnect] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [DocConnect]
GO
/****** Object:  Table [dbo].[ChuyenKhoa]    Script Date: 7/10/2026 7:08:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChuyenKhoa](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenChuyenKhoa] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoiDap]    Script Date: 7/10/2026 7:08:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoiDap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](250) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[Tuoi] [int] NOT NULL,
	[GioiTinh] [nvarchar](10) NULL,
	[DuongDanAnh] [nvarchar](500) NULL,
	[ChuyenKhoaId] [int] NULL,
	[NguoiDungId] [nvarchar](450) NULL,
	[NgayTao] [datetime] NOT NULL,
	[DaDuyet] [bit] NOT NULL,
	[AnDanh] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoSoBacSi]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoSoBacSi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NguoiDungId] [nvarchar](450) NULL,
	[ChuyenKhoaId] [int] NULL,
	[KinhNghiem] [nvarchar](max) NULL,
	[GioiThieu] [nvarchar](max) NULL,
	[DuongDanChungChi] [nvarchar](255) NOT NULL,
	[TrangThaiXacThuc] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoSoSucKhoe]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoSoSucKhoe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NguoiDungId] [nvarchar](450) NULL,
	[PhienTuVanId] [int] NULL,
	[TrieuChung] [nvarchar](max) NULL,
	[KetLuan] [nvarchar](max) NULL,
	[TenBacSi] [nvarchar](100) NULL,
	[ThoiGian] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LichHen]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LichHen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NguoiDungId] [nvarchar](450) NULL,
	[BacSiId] [int] NULL,
	[ThoiGianBatDau] [datetime] NOT NULL,
	[ThoiGianKetThuc] [datetime] NOT NULL,
	[TrangThai] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[Id] [nvarchar](450) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[MatKhauHash] [nvarchar](max) NOT NULL,
	[SoDienThoai] [nvarchar](15) NULL,
	[VaiTro] [nvarchar](20) NOT NULL,
	[TrangThai] [bit] NULL,
	[NgayTao] [datetime] NOT NULL,
	[AnhDaiDien] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhienTuVan]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhienTuVan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BacSiId] [nvarchar](450) NOT NULL,
	[BenhNhanId] [nvarchar](450) NOT NULL,
	[LichHenId] [int] NOT NULL,
	[ThoiGianBatDauThucTe] [datetime] NULL,
	[ThoiGianKetThucThucTe] [datetime] NULL,
	[KetLuan] [nvarchar](max) NULL,
	[TrangThai] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TinNhan]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinNhan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PhienTuVanId] [int] NULL,
	[NguoiGuiId] [nvarchar](450) NULL,
	[NoiDung] [nvarchar](max) NULL,
	[LoaiTinNhan] [nvarchar](10) NOT NULL,
	[ThoiGianGui] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChuyenKhoa] ON 

INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (1, N'Nội tổng hợp')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (2, N'Nhi khoa')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (3, N'Sản phụ khoa')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (4, N'Da liễu')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (5, N'Tim mạch')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (6, N'Cơ xương khớp')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (7, N'Tai Mũi Họng')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (8, N'Nhãn khoa')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (9, N'Thần kinh')
INSERT [dbo].[ChuyenKhoa] ([Id], [TenChuyenKhoa]) VALUES (10, N'Tiêu hóa')
SET IDENTITY_INSERT [dbo].[ChuyenKhoa] OFF
GO
SET IDENTITY_INSERT [dbo].[HoiDap] ON 

INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (1, N'chân', N'gãy chân', 20, N'Nữ', N'/uploads/efeff50f-c555-4f1b-b4a2-05965b863d20.jpg', 4, NULL, CAST(N'2026-07-08T01:37:42.623' AS DateTime), 1, 1)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (2, N'gvygihub', N'tfutfvkugk', 30, N'Nam', N'/uploads/7acb704e-fb3c-4588-b270-b431d1b632a8.jpg', 2, NULL, CAST(N'2026-07-08T05:30:12.023' AS DateTime), 1, 1)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (3, N'rtyudfghjksedrtgyuji', N'xdfghjzsdfghjdxcfgh', 52, N'Nam', N'/uploads/8c5ef7e5-c36b-4133-9d19-2dbe5c358033.jpg', 4, NULL, CAST(N'2026-07-08T05:38:05.827' AS DateTime), 1, 1)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (4, N'tyuioerfcdbxsnm', N'ujhgvfcdxshgfd', 52, N'Nam', N'/uploads/6f466ba7-5cb4-4c1f-9908-5c3b66a42dba.jpg', 1, NULL, CAST(N'2026-07-08T05:40:01.617' AS DateTime), 1, 1)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (5, N'RE: 4', N'ok ban', 0, N'Bác sĩ', NULL, 1, N'cfea781f-938e-4b14-9615-0739e4277be7', CAST(N'2026-07-10T03:49:50.353' AS DateTime), 1, 0)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (6, N'Em bé bị sốt, phát ban nổi mẩm đỏ', N'giờ e phải làm gì để hạ sốt và hết phát ban ạ', 23, N'Nữ', NULL, 2, N'8ef73fb0-c6b1-425f-bffc-749e0ce0b3b0', CAST(N'2026-07-10T04:06:10.460' AS DateTime), 1, 1)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (7, N'RE: 6', N'lau qua người cho bé bằng nước ấm và uống thuốc hạ sốt nếu bé không đỡ phải đưa đến bệnh viện nhé ', 0, N'Bác sĩ', NULL, 2, N'cfea781f-938e-4b14-9615-0739e4277be7', CAST(N'2026-07-10T04:07:11.533' AS DateTime), 1, 0)
INSERT [dbo].[HoiDap] ([Id], [TieuDe], [NoiDung], [Tuoi], [GioiTinh], [DuongDanAnh], [ChuyenKhoaId], [NguoiDungId], [NgayTao], [DaDuyet], [AnDanh]) VALUES (8, N'RE: 6', N'đến ngay cơ sở y tế gần nhất để thăm khám nhé ', 0, N'Bác sĩ', NULL, 2, N'cfea781f-938e-4b14-9615-0739e4277be7', CAST(N'2026-07-10T04:08:18.777' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[HoiDap] OFF
GO
SET IDENTITY_INSERT [dbo].[HoSoBacSi] ON 

INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (1, N'DR_USER_001', 1, N'15 năm kinh nghiệm', N'Nguyên trưởng khoa nội tại bệnh viện đa khoa trung ương.', N'/images/doctors/d1.jpg', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (2, N'DR_USER_002', 2, N'8 năm kinh nghiệm', N'Chuyên gia tư vấn dinh dưỡng và chăm sóc hô hấp trẻ nhỏ.', N'/uploads/certs/cc2.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (3, N'DR_USER_003', 3, N'12 năm kinh nghiệm', N'Chuyên quản lý thai sản nguy cơ cao và tư vấn hiếm muộn.', N'/uploads/certs/cc3.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (4, N'DR_USER_004', 4, N'10 năm kinh nghiệm', N'Chuyên điều trị mụn trứng cá nặng, sẹo rỗ và da liễu thẩm mỹ.', N'/uploads/certs/cc4.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (5, N'DR_USER_005', 5, N'20 năm kinh nghiệm', N'Chuyên gia đầu ngành về điều trị suy tim, bệnh mạch vành và tăng huyết áp.', N'/uploads/certs/cc5.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (6, N'DR_USER_006', 6, N'7 năm kinh nghiệm', N'Điều trị hiệu quả thoái hóa cột sống, thoát vị đĩa đệm bằng nội khoa.', N'/uploads/certs/cc6.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (7, N'DR_USER_007', 7, N'9 năm kinh nghiệm', N'Chuyên nội soi Tai mũi họng, xử lý viêm xoang, viêm amidan mạn tính.', N'/uploads/certs/cc7.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (8, N'DR_USER_008', 8, N'11 năm kinh nghiệm', N'Khám và kiểm soát cận thị học đường, điều trị các bệnh lý về giác mạc.', N'/uploads/certs/cc8.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (9, N'DR_USER_009', 9, N'14 năm kinh nghiệm', N'Tư vấn sâu về chứng mất ngủ, đau nửa đầu mạn tính và rối loạn lo âu.', N'/uploads/certs/cc9.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (10, N'DR_USER_010', 10, N'16 năm kinh nghiệm', N'Chuyên trị trào ngược dạ dày thực quản (GERD), viêm loét đại tràng.', N'/uploads/certs/cc10.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (11, N'DR_USER_011', 1, N'5 năm kinh nghiệm', N'Bác sĩ trẻ năng động, chuyên khám sức khỏe tổng quát định kỳ.', N'/uploads/certs/cc11.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (12, N'DR_USER_012', 2, N'12 năm kinh nghiệm', N'Bác sĩ Nhi khoa tận tâm, yêu trẻ, tư vấn tiêm chủng và bệnh mùa dịch.', N'/uploads/certs/cc12.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (13, N'DR_USER_013', 3, N'6 năm kinh nghiệm', N'Tư vấn chăm sóc sức khỏe sinh sản và kế hoạch hóa gia đình.', N'/uploads/certs/cc13.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (14, N'DR_USER_014', 4, N'18 năm kinh nghiệm', N'Chuyên gia điều trị viêm da cơ địa, vảy nến và phục hồi da nhiễm corticoid.', N'/uploads/certs/cc14.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (15, N'DR_USER_015', 5, N'13 năm kinh nghiệm', N'Tư vấn dự phòng biến chứng tim mạch do tiểu đường và mỡ máu.', N'/uploads/certs/cc15.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (16, N'DR_USER_016', 6, N'9 năm kinh nghiệm', N'Chẩn đoán, tiêm khớp điều trị viêm khớp dạng thấp và thoái hóa khớp gối.', N'/uploads/certs/cc16.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (17, N'DR_USER_017', 7, N'15 năm kinh nghiệm', N'Điều trị chuyên sâu viêm tai giữa và phẫu thuật nội soi xoang.', N'/uploads/certs/cc17.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (18, N'DR_USER_018', 8, N'6 năm kinh nghiệm', N'Tư vấn phẫu thuật Lasik tật khúc xạ và chăm sóc mắt văn phòng.', N'/uploads/certs/cc18.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (19, N'DR_USER_019', 9, N'22 năm kinh nghiệm', N'Điều trị suy giảm trí nhớ, Parkinson và các tai biến mạch máu não nhẹ.', N'/uploads/certs/cc19.pdf', N'1')
INSERT [dbo].[HoSoBacSi] ([Id], [NguoiDungId], [ChuyenKhoaId], [KinhNghiem], [GioiThieu], [DuongDanChungChi], [TrangThaiXacThuc]) VALUES (20, N'DR_USER_020', 10, N'8 năm kinh nghiệm', N'Tư vấn nội soi dạ dày không đau, hội chứng ruột kích thích (IBS).', N'/uploads/certs/cc20.pdf', N'1')
SET IDENTITY_INSERT [dbo].[HoSoBacSi] OFF
GO
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'8ef73fb0-c6b1-425f-bffc-749e0ce0b3b0', N'Bùi Thị Thương ', N'thuong123@gmail.com', N'AQAAAAIAAYagAAAAENFG5bJCkGieKmI30S38nkeJaSNmeCox/zwksFmLfr5BVK3TXL/x6SZXFgnDbJDmWg==', N'0982813066', N'Customer', 1, CAST(N'2026-07-07T23:44:47.080' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'cfea781f-938e-4b14-9615-0739e4277be7', N'Nguyễn Văn Thành ', N'thanh@gmail.com', N'AQAAAAIAAYagAAAAEExiDy5ZDSxpekSbyCWtPtcioJ5vjy6wAuWlD/vqeQKe3wu67Qi3rhG4sJxmTURIFQ==', N'0982813067', N'Doctor', 1, CAST(N'2026-07-10T03:15:34.007' AS DateTime), N'/images/default-avatar.png')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_001', N'BS. Hoàng Thị Xuân', N'xuan.hoang@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345678', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.423' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_002', N'BS. Hồ Minh Tâm', N'tam.ho@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345679', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.430' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_003', N'BS. Dương Thị Hạnh', N'hanh.duong@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345680', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.430' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_004', N'BS. Nguyễn Văn An', N'an.nguyen@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345681', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_005', N'BS. Trần Thị Mai', N'mai.tran@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345682', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_006', N'BS. Lê Văn Bình', N'binh.le@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345683', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_007', N'BS. Phạm Minh Đức', N'duc.pham@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345684', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_008', N'BS. Đặng Thu Thảo', N'thao.dang@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345685', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_009', N'BS. Bùi Quang Huy', N'huy.bui@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345686', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_010', N'BS. Ngô Bảo Châu', N'chau.ngo@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345687', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_011', N'BS. Võ Văn Thưởng', N'thuong.vo@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345688', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_012', N'BS. Nguyễn Thị Lam', N'lam.nguyen@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345689', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_013', N'BS. Phan Thành', N'thanh.phan@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345690', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_014', N'BS. Đỗ Kim Liên', N'lien.do@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345691', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_015', N'BS. Vương Đình Long', N'long.vuong@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345692', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_016', N'BS. Nguyễn Minh Phúc', N'phuc.minh@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345693', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_017', N'BS. Phạm Bình Minh', N'minh.binh@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345694', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_018', N'BS. Nguyễn Hòa Bình', N'binh.hoa@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345695', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_019', N'BS. Tô Lâm', N'lam.to@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345696', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
INSERT [dbo].[NguoiDung] ([Id], [HoTen], [Email], [MatKhauHash], [SoDienThoai], [VaiTro], [TrangThai], [NgayTao], [AnhDaiDien]) VALUES (N'DR_USER_020', N'BS. Lương Cường', N'cuong.luong@docconect.com', N'AQAAAAEAACcQAAAAE...', N'0912345697', N'Doctor', 1, CAST(N'2026-07-08T02:43:53.433' AS DateTime), N'/images/doctors/d1.jpg')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NguoiDun__A9D10534295786B8]    Script Date: 7/10/2026 7:08:33 AM ******/
ALTER TABLE [dbo].[NguoiDung] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__PhienTuV__F9299197D063CC2B]    Script Date: 7/10/2026 7:08:33 AM ******/
ALTER TABLE [dbo].[PhienTuVan] ADD UNIQUE NONCLUSTERED 
(
	[LichHenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HoiDap] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[HoiDap] ADD  DEFAULT ((0)) FOR [DaDuyet]
GO
ALTER TABLE [dbo].[HoiDap] ADD  DEFAULT ((1)) FOR [AnDanh]
GO
ALTER TABLE [dbo].[HoSoBacSi] ADD  DEFAULT (N'Chờ duyệt') FOR [TrangThaiXacThuc]
GO
ALTER TABLE [dbo].[LichHen] ADD  DEFAULT (N'Chờ diễn ra') FOR [TrangThai]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[PhienTuVan] ADD  DEFAULT (N'Đang diễn ra') FOR [TrangThai]
GO
ALTER TABLE [dbo].[HoiDap]  WITH CHECK ADD  CONSTRAINT [FK_HoiDap_NguoiDung] FOREIGN KEY([NguoiDungId])
REFERENCES [dbo].[NguoiDung] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[HoiDap] CHECK CONSTRAINT [FK_HoiDap_NguoiDung]
GO
ALTER TABLE [dbo].[HoSoBacSi]  WITH CHECK ADD FOREIGN KEY([ChuyenKhoaId])
REFERENCES [dbo].[ChuyenKhoa] ([Id])
GO
ALTER TABLE [dbo].[HoSoBacSi]  WITH CHECK ADD FOREIGN KEY([NguoiDungId])
REFERENCES [dbo].[NguoiDung] ([Id])
GO
ALTER TABLE [dbo].[HoSoSucKhoe]  WITH CHECK ADD FOREIGN KEY([NguoiDungId])
REFERENCES [dbo].[NguoiDung] ([Id])
GO
ALTER TABLE [dbo].[HoSoSucKhoe]  WITH CHECK ADD FOREIGN KEY([PhienTuVanId])
REFERENCES [dbo].[PhienTuVan] ([Id])
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD FOREIGN KEY([BacSiId])
REFERENCES [dbo].[HoSoBacSi] ([Id])
GO
ALTER TABLE [dbo].[LichHen]  WITH CHECK ADD FOREIGN KEY([NguoiDungId])
REFERENCES [dbo].[NguoiDung] ([Id])
GO
ALTER TABLE [dbo].[PhienTuVan]  WITH CHECK ADD FOREIGN KEY([LichHenId])
REFERENCES [dbo].[LichHen] ([Id])
GO
ALTER TABLE [dbo].[PhienTuVan]  WITH CHECK ADD FOREIGN KEY([BacSiId])
REFERENCES [dbo].[NguoiDung] ([Id]) ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[PhienTuVan]  WITH CHECK ADD FOREIGN KEY([BenhNhanId])
REFERENCES [dbo].[NguoiDung] ([Id]) ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[TinNhan]  WITH CHECK ADD FOREIGN KEY([NguoiGuiId])
REFERENCES [dbo].[NguoiDung] ([Id])
GO
ALTER TABLE [dbo].[TinNhan]  WITH CHECK ADD FOREIGN KEY([PhienTuVanId])
REFERENCES [dbo].[PhienTuVan] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[GetChiTietBacSi]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [dbo].[GetChiTietBacSi]
GO
CREATE PROCEDURE [dbo].[GetChiTietBacSi]
    @BacSiId NVARCHAR(50) -- Nhận mã kiểu chuỗi (ví dụ: 'DR_USER_001')
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm khám chữa bệnh.') AS KinhNghiem,
        ISNULL(h.GioiThieu, N'Đang cập nhật thông tin giới thiệu.') AS GioiThieu,
        n.TrangThai
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor' AND n.Id = @BacSiId; -- Khớp chính xác theo Id dạng chuỗi của bạn
END
GO
/****** Object:  StoredProcedure [dbo].[GetDanhSachBacSi]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [dbo].[GetDanhSachBacSi]
GO

-- 2. Tạo thủ tục mới chống lỗi ép kiểu dữ liệu 'Hoạt động' sang bit và hỗ trợ tìm kiếm từ khóa
CREATE PROCEDURE [dbo].[GetDanhSachBacSi]
    @ChuyenKhoaId INT = NULL,
    @Keyword NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm') AS KinhNghiem,
        ISNULL(h.GioiThieu, '') AS GioiThieu,
        n.TrangThai
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor'
      -- Lọc động theo chuyên khoa
      AND (@ChuyenKhoaId IS NULL OR @ChuyenKhoaId = 0 OR h.ChuyenKhoaId = @ChuyenKhoaId)
      -- Tìm kiếm theo từ khóa
      AND (@Keyword IS NULL OR @Keyword = '' OR n.HoTen LIKE '%' + @Keyword + '%' OR c.TenChuyenKhoa LIKE '%' + @Keyword + '%');
END
GO
/****** Object:  StoredProcedure [dbo].[GetTop5BacSi]    Script Date: 7/10/2026 7:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [dbo].[GetTop5BacSi]
GO
CREATE PROCEDURE [dbo].[GetTop5BacSi]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        h.NguoiDungId AS Id,
        n.HoTen,
        n.Email,
        ISNULL(n.SoDienThoai, '') AS SoDienThoai,
        ISNULL(n.AnhDaiDien, '/images/default-avatar.png') AS AnhDaiDien,
        c.TenChuyenKhoa,
        h.ChuyenKhoaId,
        ISNULL(h.KinhNghiem, N'Đang cập nhật kinh nghiệm') AS KinhNghiem,
        ISNULL(h.GioiThieu, '') AS GioiThieu,
        n.TrangThai
    FROM HoSoBacSi h
    INNER JOIN NguoiDung n ON h.NguoiDungId = n.Id
    INNER JOIN ChuyenKhoa c ON h.ChuyenKhoaId = c.Id
    WHERE n.VaiTro = 'Doctor'
      -- SỬA TẠI ĐÂY: Vì TrangThai là kiểu BIT (bool), chỉ cần check = 1 (hoặc true)
      AND n.TrangThai = 1
    ORDER BY n.NgayTao DESC; 
END
GO
USE [master]
GO
ALTER DATABASE [DocConnect] SET  READ_WRITE 
GO
