USE [master]
GO
/****** Object:  Database [QLUser]    Script Date: 11/4/2024 4:40:48 AM ******/
CREATE DATABASE [QLUser]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLUser', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLUser.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QLUser_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLUser_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QLUser] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLUser].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLUser] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLUser] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLUser] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLUser] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLUser] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLUser] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLUser] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLUser] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLUser] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLUser] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLUser] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLUser] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLUser] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLUser] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLUser] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QLUser] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLUser] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLUser] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLUser] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLUser] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLUser] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLUser] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLUser] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QLUser] SET  MULTI_USER 
GO
ALTER DATABASE [QLUser] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLUser] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLUser] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLUser] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QLUser] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QLUser] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [QLUser] SET QUERY_STORE = ON
GO
ALTER DATABASE [QLUser] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QLUser]
GO
/****** Object:  Table [dbo].[BanBe]    Script Date: 11/4/2024 4:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BanBe](
	[MaNguoiDung] [int] NOT NULL,
	[MaBanBe] [int] NOT NULL,
	[TrangThai] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 11/4/2024 4:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[MaNguoiDung] [int] IDENTITY(1,1) NOT NULL,
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](255) NOT NULL,
	[HoTen] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[SoDienThoai] [nvarchar](15) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[NgayTao] [datetime] NULL,
	[NgaySinh] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung_SoThich]    Script Date: 11/4/2024 4:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung_SoThich](
	[MaNguoiDung] [int] NOT NULL,
	[MaSoThich] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC,
	[MaSoThich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoThich]    Script Date: 11/4/2024 4:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoThich](
	[MaSoThich] [int] IDENTITY(1,1) NOT NULL,
	[TenSoThich] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSoThich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TinNhan]    Script Date: 11/4/2024 4:40:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinNhan](
	[MaTinNhan] [int] IDENTITY(1,1) NOT NULL,
	[MaNguoiGui] [int] NOT NULL,
	[MaNguoiNhan] [int] NOT NULL,
	[NoiDung] [nvarchar](max) NULL,
	[ThoiGian] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaTinNhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (1, 3, 0)
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (3, 1, 0)
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (1, 2, 0)
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (2, 1, 1)
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (2, 3, 0)
INSERT [dbo].[BanBe] ([MaNguoiDung], [MaBanBe], [TrangThai]) VALUES (3, 2, 0)
GO
SET IDENTITY_INSERT [dbo].[NguoiDung] ON 

INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (1, N'PhamTheTai', N'180404', N'Phạm Thế Tài', N'thetai443@gmail.com', N'0869718835', N'Thái Bình', CAST(N'2024-10-24T19:01:19.747' AS DateTime), CAST(N'2004-04-18' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (2, N'PhamTheTai1', N'180405', N'Phạm Thế Tài2', N'thetai444@gmail.com', N'0869718836', N'Hà Nội', CAST(N'2024-10-24T19:01:19.747' AS DateTime), CAST(N'2004-04-19' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (3, N'PhamTheTai2', N'180406', N'Phạm Thế Tài3', N'thetai445@gmail.com', N'0869718837', N'TP Hồ Chí Minh', CAST(N'2024-10-24T19:01:19.747' AS DateTime), CAST(N'2000-04-20' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (4, N'phamthetai9', N'123456', N'Phạm Thế Tài9', NULL, NULL, N'Kon Tum', CAST(N'2024-11-01T19:30:18.273' AS DateTime), CAST(N'1992-04-20' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (5, N'tranchienTG', N'123456', N'Trần Chiến', NULL, NULL, N'Quảng Ninh', CAST(N'2024-11-01T23:07:59.127' AS DateTime), CAST(N'1991-04-29' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (6, N'TESt1', N'123456', N'Trần Văn An', NULL, NULL, N'Thái Bình', CAST(N'2024-11-03T03:43:36.870' AS DateTime), CAST(N'2004-09-12' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (7, N'TESt2', N'123456', N'Phạm Văn Bình', NULL, NULL, N'Bắc Ninh', CAST(N'2024-11-03T03:46:11.830' AS DateTime), CAST(N'1995-09-12' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (8, N'TESt3', N'123456', N'Lê Văn Cường', NULL, NULL, N'Nghệ An', CAST(N'2024-11-03T03:46:11.830' AS DateTime), CAST(N'2004-05-14' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (9, N'TESt4', N'123456', N'Đào Văn Dũng', NULL, NULL, N'Thái Bình', CAST(N'2024-11-03T03:46:11.830' AS DateTime), CAST(N'2003-09-20' AS Date))
INSERT [dbo].[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhau], [HoTen], [Email], [SoDienThoai], [DiaChi], [NgayTao], [NgaySinh]) VALUES (10, N'TESt5', N'123456', N'Nguyễn Văn Em', NULL, NULL, N'Nam Định', CAST(N'2024-11-03T03:46:11.830' AS DateTime), CAST(N'2004-09-20' AS Date))
SET IDENTITY_INSERT [dbo].[NguoiDung] OFF
GO
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (1, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (1, 4)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (1, 5)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (1, 7)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (1, 9)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (2, 1)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (2, 2)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (2, 8)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (2, 9)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (3, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (3, 9)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (4, 5)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (5, 2)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (5, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (5, 4)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (5, 10)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (6, 2)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (6, 4)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (6, 6)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (6, 8)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (7, 1)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (7, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (7, 5)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (7, 7)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (7, 9)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (8, 2)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (8, 4)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (8, 6)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (8, 8)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (8, 10)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (9, 1)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (9, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (9, 5)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (9, 7)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (9, 9)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 1)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 2)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 3)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 4)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 5)
INSERT [dbo].[NguoiDung_SoThich] ([MaNguoiDung], [MaSoThich]) VALUES (10, 6)
GO
SET IDENTITY_INSERT [dbo].[SoThich] ON 

INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (9, N'Chơi game')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (4, N'Chơi thể thao')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (7, N'Chụp ảnh')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (1, N'Đọc sách')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (3, N'Du lịch')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (10, N'Hát')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (8, N'Mua sắm')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (6, N'Nấu ăn')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (5, N'Nghe nhạc')
INSERT [dbo].[SoThich] ([MaSoThich], [TenSoThich]) VALUES (2, N'Xem phim')
SET IDENTITY_INSERT [dbo].[SoThich] OFF
GO
SET IDENTITY_INSERT [dbo].[TinNhan] ON 

INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (1, 1, 2, N'Kiểm tra database', CAST(N'2024-10-29T21:00:04.737' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (2, 2, 1, N'Xác nhận kiểm tra database', CAST(N'2024-10-29T21:00:04.737' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (3, 1, 2, N'Hoàn tất kiểm tra database', CAST(N'2024-10-29T21:00:04.737' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (4, 1, 2, N'hello ?', CAST(N'2024-10-29T22:51:10.397' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (5, 2, 1, N'ok tôi nhận được rồi ', CAST(N'2024-10-29T22:51:29.663' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (6, 1, 2, N'1', CAST(N'2024-10-29T22:51:34.857' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (7, 2, 1, N'CHECK', CAST(N'2024-10-29T23:00:43.313' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (8, 1, 2, N'check', CAST(N'2024-10-29T23:05:17.187' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (9, 2, 1, N'hello ?', CAST(N'2024-10-29T23:07:12.560' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (10, 1, 2, N'check', CAST(N'2024-10-30T14:11:26.647' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (11, 1, 2, N'check', CAST(N'2024-10-31T01:09:11.870' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (12, 1, 2, N'check', CAST(N'2024-10-31T01:31:58.633' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (13, 2, 1, N'hello ?', CAST(N'2024-11-01T18:25:45.383' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (14, 1, 2, N'ok', CAST(N'2024-11-01T18:25:59.557' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (15, 1, 2, N'hi', CAST(N'2024-11-03T22:54:51.890' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (16, 1, 3, N'xin chào xin chào ?', CAST(N'2024-11-04T04:02:35.843' AS DateTime))
INSERT [dbo].[TinNhan] ([MaTinNhan], [MaNguoiGui], [MaNguoiNhan], [NoiDung], [ThoiGian]) VALUES (17, 3, 1, N'yes ser', CAST(N'2024-11-04T04:02:54.083' AS DateTime))
SET IDENTITY_INSERT [dbo].[TinNhan] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Unique_UserName]    Script Date: 11/4/2024 4:40:48 AM ******/
ALTER TABLE [dbo].[NguoiDung] ADD  CONSTRAINT [Unique_UserName] UNIQUE NONCLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__NguoiDun__55F68FC0C4E628D6]    Script Date: 11/4/2024 4:40:48 AM ******/
ALTER TABLE [dbo].[NguoiDung] ADD UNIQUE NONCLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__SoThich__9E1824A69DCFE6A5]    Script Date: 11/4/2024 4:40:48 AM ******/
ALTER TABLE [dbo].[SoThich] ADD UNIQUE NONCLUSTERED 
(
	[TenSoThich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BanBe] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[TinNhan] ADD  DEFAULT (getdate()) FOR [ThoiGian]
GO
ALTER TABLE [dbo].[BanBe]  WITH CHECK ADD FOREIGN KEY([MaBanBe])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NguoiDung_SoThich]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NguoiDung_SoThich]  WITH CHECK ADD FOREIGN KEY([MaSoThich])
REFERENCES [dbo].[SoThich] ([MaSoThich])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TinNhan]  WITH CHECK ADD FOREIGN KEY([MaNguoiGui])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TinNhan]  WITH CHECK ADD FOREIGN KEY([MaNguoiNhan])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[BanBe]  WITH CHECK ADD  CONSTRAINT [CK_TrangThai] CHECK  (([TrangThai]=(0) OR [TrangThai]=(1)))
GO
ALTER TABLE [dbo].[BanBe] CHECK CONSTRAINT [CK_TrangThai]
GO
USE [master]
GO
ALTER DATABASE [QLUser] SET  READ_WRITE 
GO
