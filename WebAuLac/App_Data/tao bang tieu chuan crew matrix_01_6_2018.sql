USE [AuLac]
GO
/****** Object:  Table [dbo].[TieuChuanCrewMatrix]    Script Date: 6/1/2018 4:09:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TieuChuanCrewMatrix](
	[TieuChuanID] [int] IDENTITY(1,1) NOT NULL,
	[TieuChuanName] [nvarchar](50) NULL,
	[WithOperator_QuanLyBoong] [decimal](2, 1) NULL,
	[InRank_QuanLyBoong] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_QuanLyBoong] [decimal](2, 1) NULL,
	[AllTypeOfTanker_QuanLyBoong] [decimal](2, 1) NULL,
	[WithOperator_ThuyenTruong] [decimal](2, 1) NULL,
	[InRank_ThuyenTruong] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_ThuyenTruong] [decimal](2, 1) NULL,
	[AllTypeOfTanker_ThuyenTruong] [decimal](2, 1) NULL,
	[WithOperator_DaiPho] [decimal](2, 1) NULL,
	[InRank_DaiPho] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_DaiPho] [decimal](2, 1) NULL,
	[AllTypeOfTanker_DaiPho] [decimal](2, 1) NULL,
	[WithOperator_QuanLyMay] [decimal](2, 1) NULL,
	[InRank_QuanLyMay] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_QuanLyMay] [decimal](2, 1) NULL,
	[AllTypeOfTanker_QuanLyMay] [decimal](2, 1) NULL,
	[WithOperator_MayTruong] [decimal](2, 1) NULL,
	[InRank_MayTruong] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_MayTruong] [decimal](2, 1) NULL,
	[AllTypeOfTanker_MayTruong] [decimal](2, 1) NULL,
	[WithOperator_May2] [decimal](2, 1) NULL,
	[InRank_May2] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_May2] [decimal](2, 1) NULL,
	[AllTypeOfTanker_May2] [decimal](2, 1) NULL,
	[WithOperator_VanHanhBoong] [decimal](2, 1) NULL,
	[InRank_VanHanhBoong] [decimal](2, 1) NULL,
	[ThisTypeOfTanker_VanHanhBoong] [decimal](2, 1) NULL,
	[AllTypeOfTanker_VanHanhBoong] [decimal](2, 1) NULL,
	[WithOperator_VanHanhMay] [decimal](2, 1) NULL,
	[InRank_VanHanhMay] [decimal](2, 1) NULL,
	[ThisTypeTanker_VanHanhMay] [decimal](2, 1) NULL,
	[AllTypeOfTanker_VanHanhMay] [decimal](2, 1) NULL,
 CONSTRAINT [PK_TieuChuanCrewMatrix] PRIMARY KEY CLUSTERED 
(
	[TieuChuanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[TieuChuanCrewMatrix] ON 

INSERT [dbo].[TieuChuanCrewMatrix] ([TieuChuanID], [TieuChuanName], [WithOperator_QuanLyBoong], [InRank_QuanLyBoong], [ThisTypeOfTanker_QuanLyBoong], [AllTypeOfTanker_QuanLyBoong], [WithOperator_ThuyenTruong], [InRank_ThuyenTruong], [ThisTypeOfTanker_ThuyenTruong], [AllTypeOfTanker_ThuyenTruong], [WithOperator_DaiPho], [InRank_DaiPho], [ThisTypeOfTanker_DaiPho], [AllTypeOfTanker_DaiPho], [WithOperator_QuanLyMay], [InRank_QuanLyMay], [ThisTypeOfTanker_QuanLyMay], [AllTypeOfTanker_QuanLyMay], [WithOperator_MayTruong], [InRank_MayTruong], [ThisTypeOfTanker_MayTruong], [AllTypeOfTanker_MayTruong], [WithOperator_May2], [InRank_May2], [ThisTypeOfTanker_May2], [AllTypeOfTanker_May2], [WithOperator_VanHanhBoong], [InRank_VanHanhBoong], [ThisTypeOfTanker_VanHanhBoong], [AllTypeOfTanker_VanHanhBoong], [WithOperator_VanHanhMay], [InRank_VanHanhMay], [ThisTypeTanker_VanHanhMay], [AllTypeOfTanker_VanHanhMay]) VALUES (1, N'Tiêu chuẩn chung', CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)))
INSERT [dbo].[TieuChuanCrewMatrix] ([TieuChuanID], [TieuChuanName], [WithOperator_QuanLyBoong], [InRank_QuanLyBoong], [ThisTypeOfTanker_QuanLyBoong], [AllTypeOfTanker_QuanLyBoong], [WithOperator_ThuyenTruong], [InRank_ThuyenTruong], [ThisTypeOfTanker_ThuyenTruong], [AllTypeOfTanker_ThuyenTruong], [WithOperator_DaiPho], [InRank_DaiPho], [ThisTypeOfTanker_DaiPho], [AllTypeOfTanker_DaiPho], [WithOperator_QuanLyMay], [InRank_QuanLyMay], [ThisTypeOfTanker_QuanLyMay], [AllTypeOfTanker_QuanLyMay], [WithOperator_MayTruong], [InRank_MayTruong], [ThisTypeOfTanker_MayTruong], [AllTypeOfTanker_MayTruong], [WithOperator_May2], [InRank_May2], [ThisTypeOfTanker_May2], [AllTypeOfTanker_May2], [WithOperator_VanHanhBoong], [InRank_VanHanhBoong], [ThisTypeOfTanker_VanHanhBoong], [AllTypeOfTanker_VanHanhBoong], [WithOperator_VanHanhMay], [InRank_VanHanhMay], [ThisTypeTanker_VanHanhMay], [AllTypeOfTanker_VanHanhMay]) VALUES (2, N'Chervon', CAST(3.0 AS Decimal(2, 1)), CAST(4.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(6.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(2.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(3.0 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.0 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)), CAST(1.5 AS Decimal(2, 1)))
SET IDENTITY_INSERT [dbo].[TieuChuanCrewMatrix] OFF
