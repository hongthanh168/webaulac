alter table [dbo].[DIC_DEPARTMENT]
add IsUsed bit null
go
update [dbo].[DIC_DEPARTMENT]
set IsUsed =1
go
------------------------------------------
--thêm bảng điều động bậc thang
CREATE TABLE [dbo].[DieuDongBacThang](
	[DieuDongBacThangID] [int] IDENTITY(1,1) NOT NULL,
	[Tau] [nvarchar](50) NULL,
	[DepartmentID] [int] NULL,
	[PositionID] [int] NULL,
	[NguoiDangDi] [nvarchar](200) NULL,
	[NguoiDangDiID] [int] NULL,
	[NgayLenTau] [smalldatetime] NULL,
	[ThangDiTau] [int] NULL,
	[NguoiThayThe] [nvarchar](200) NULL,
	[NguoiThayTheID] [int] NULL,
	[NgayThayThe] [smalldatetime] NULL,
	[ThangDiTauThayThe] [int] NULL,
 CONSTRAINT [PK_DieuDongBacThang] PRIMARY KEY CLUSTERED 
(
	[DieuDongBacThangID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
-------------------------------------------
--tạo dữ liệu ban đầu cho điều động bậc thang
ALTER procedure [dbo].[sp_T_TaoDuLieuBanDauChoDieuDong]
as
	begin
		insert into DieuDongBacThang ([Tau]
           ,[DepartmentID]
           ,[PositionID]
           ,[NguoiDangDi]
           ,[NguoiDangDiID]
           ,[NgayLenTau]
           ,[ThangDiTau]
           ,[NguoiThayThe]
           ,[NguoiThayTheID]
		   ,[NgayThayThe]
           ,[ThangDiTauThayThe])
			select d.Description, d.DepartmentID, v.PositionID, v.FirstName + ' ' + v.LastName, v.EmployeeID, v.NgayXuongTau, 8, '', null,
				DATEADD(MONTH, 8, v.NgayXuongTau), 8
			from DIC_DEPARTMENT d, viewHRM_EMPLOYMENTHISTORY v
			where d.DepartmentID = v.DepartmentID and d.IsUsed =1 and d.Description is not null and v.PositionID between 20 and 27 
			and LEN (d.Description )<10
	end
GO
---------------------------------
---lấy bảng lịch tàu trong điều động bậc thang
USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_T_LayLichTauTrongDieuDongBacThang]    Script Date: 4/19/2024 4:08:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_T_LayLichTauTrongDieuDongBacThang]
@ngayTinh smalldatetime
as
	begin
		declare @ngayMin as smalldatetime, @ngayMax as smalldatetime
		set @ngayMin = DATEADD(MONTH, -10, @ngayTinh)
		set @ngayMax = DATEADD(MONTH, 10, @ngayTinh)
		--trước tiên là lấy danh sách lịch kiểm tra tàu
		--Descrition là viết tắt tên tàu
		--format kết quả sẽ là Tên loại kiểm tra_ngày kiểm tra_id lịch kiểm tra
		select 'LICHKIEMTRA' as Loai, kq.DepartmentID, kq.Description,
			MAX(CASE WHEN cot1 IS NOT NULL THEN cot1 ELSE '' END) AS cot1,
			MAX(CASE WHEN cot2 IS NOT NULL THEN cot2 ELSE '' END) AS cot2,
			MAX(CASE WHEN cot3 IS NOT NULL THEN cot3 ELSE '' END) AS cot3,
			MAX(CASE WHEN cot4 IS NOT NULL THEN cot4 ELSE '' END) AS cot4,
			MAX(CASE WHEN cot5 IS NOT NULL THEN cot5 ELSE '' END) AS cot5,
			MAX(CASE WHEN cot6 IS NOT NULL THEN cot6 ELSE '' END) AS cot6,
			MAX(CASE WHEN cot7 IS NOT NULL THEN cot7 ELSE '' END) AS cot7,
			MAX(CASE WHEN cot8 IS NOT NULL THEN cot8 ELSE '' END) AS cot8,
			MAX(CASE WHEN cot9 IS NOT NULL THEN cot9 ELSE '' END) AS cot9,
			MAX(CASE WHEN cot10 IS NOT NULL THEN cot10 ELSE '' END) AS cot10,
			MAX(CASE WHEN cot11 IS NOT NULL THEN cot11 ELSE '' END) AS cot11,
			MAX(CASE WHEN cot12 IS NOT NULL THEN cot12 ELSE '' END) AS cot12,
			MAX(CASE WHEN cot13 IS NOT NULL THEN cot13 ELSE '' END) AS cot13,
			MAX(CASE WHEN cot14 IS NOT NULL THEN cot14 ELSE '' END) AS cot14,
			MAX(CASE WHEN cot15 IS NOT NULL THEN cot15 ELSE '' END) AS cot15,
			MAX(CASE WHEN cot16 IS NOT NULL THEN cot16 ELSE '' END) AS cot16,
			MAX(CASE WHEN cot17 IS NOT NULL THEN cot17 ELSE '' END) AS cot17,
			MAX(CASE WHEN cot18 IS NOT NULL THEN cot18 ELSE '' END) AS cot18,
			MAX(CASE WHEN cot19 IS NOT NULL THEN cot19 ELSE '' END) AS cot19,
			MAX(CASE WHEN cot20 IS NOT NULL THEN cot20 ELSE '' END) AS cot20,
			MAX(CASE WHEN cot21 IS NOT NULL THEN cot21 ELSE '' END) AS cot21
		from
		(select  'LICHKIEMTRA' as Loai, li.DepartmentID as DepartmentID, p.Description as Description,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 10, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot1,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 9, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot2,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 8, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot3,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 7, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot4,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 6, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot5,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 5, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot6,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 4, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot7,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 3, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot8,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 2, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot9,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 1, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot10,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 0, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot11,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -1, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot12,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -2, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot13,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -3, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot14,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -4, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot15,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -5, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot16,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -6, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot17,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -7, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot18,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -8, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot19,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -9, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot20,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -10, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot21
		from [dbo].[LichKiemTra] li, [dbo].[LoaiKiemTra] l, [dbo].[DIC_DEPARTMENT] p
		where li.LoaiKiemTraID = l.LoaiKiemTraID and p.DepartmentID = li.DepartmentID and p.ParentID =8 and p.IsUsed =1
		and DATEFROMPARTS(li.Nam, li.Thang, li.Ngay) between @ngayMin and @ngayMax) as kq
		GROUP BY kq.DepartmentID, kq.Description
	end
	go
-----------------------------------------------------------------
--lấy lịch tàu để hiển thị dạng điều động bậc thang
ALTER procedure [dbo].[sp_T_LayLichTauTrongDieuDongBacThang]
@ngayTinh smalldatetime
as
	begin
		declare @ngayMin as smalldatetime, @ngayMax as smalldatetime
		set @ngayMin = DATEADD(MONTH, -10, @ngayTinh)
		set @ngayMax = DATEADD(MONTH, 10, @ngayTinh)
		--trước tiên là lấy danh sách lịch kiểm tra tàu
		--Descrition là viết tắt tên tàu
		--format kết quả sẽ là Tên loại kiểm tra_ngày kiểm tra_id lịch kiểm tra
		select 'LICHKIEMTRA' as Loai, kq.DepartmentID, kq.Description,
			MAX(CASE WHEN cot1 IS NOT NULL THEN cot1 ELSE '' END) AS cot1,
			MAX(CASE WHEN cot2 IS NOT NULL THEN cot2 ELSE '' END) AS cot2,
			MAX(CASE WHEN cot3 IS NOT NULL THEN cot3 ELSE '' END) AS cot3,
			MAX(CASE WHEN cot4 IS NOT NULL THEN cot4 ELSE '' END) AS cot4,
			MAX(CASE WHEN cot5 IS NOT NULL THEN cot5 ELSE '' END) AS cot5,
			MAX(CASE WHEN cot6 IS NOT NULL THEN cot6 ELSE '' END) AS cot6,
			MAX(CASE WHEN cot7 IS NOT NULL THEN cot7 ELSE '' END) AS cot7,
			MAX(CASE WHEN cot8 IS NOT NULL THEN cot8 ELSE '' END) AS cot8,
			MAX(CASE WHEN cot9 IS NOT NULL THEN cot9 ELSE '' END) AS cot9,
			MAX(CASE WHEN cot10 IS NOT NULL THEN cot10 ELSE '' END) AS cot10,
			MAX(CASE WHEN cot11 IS NOT NULL THEN cot11 ELSE '' END) AS cot11,
			MAX(CASE WHEN cot12 IS NOT NULL THEN cot12 ELSE '' END) AS cot12,
			MAX(CASE WHEN cot13 IS NOT NULL THEN cot13 ELSE '' END) AS cot13,
			MAX(CASE WHEN cot14 IS NOT NULL THEN cot14 ELSE '' END) AS cot14,
			MAX(CASE WHEN cot15 IS NOT NULL THEN cot15 ELSE '' END) AS cot15,
			MAX(CASE WHEN cot16 IS NOT NULL THEN cot16 ELSE '' END) AS cot16,
			MAX(CASE WHEN cot17 IS NOT NULL THEN cot17 ELSE '' END) AS cot17,
			MAX(CASE WHEN cot18 IS NOT NULL THEN cot18 ELSE '' END) AS cot18,
			MAX(CASE WHEN cot19 IS NOT NULL THEN cot19 ELSE '' END) AS cot19,
			MAX(CASE WHEN cot20 IS NOT NULL THEN cot20 ELSE '' END) AS cot20,
			MAX(CASE WHEN cot21 IS NOT NULL THEN cot21 ELSE '' END) AS cot21
		from
		(select  'LICHKIEMTRA' as Loai, li.DepartmentID as DepartmentID, p.Description as Description,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 10, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot1,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 9, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot2,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 8, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot3,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 7, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot4,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 6, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot5,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 5, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot6,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 4, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot7,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 3, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot8,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 2, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot9,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 1, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot10,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = 0, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot11,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -1, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot12,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -2, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot13,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -3, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot14,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -4, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot15,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -5, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot16,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -6, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot17,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -7, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot18,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -8, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot19,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -9, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot20,
			IIF(DATEDIFF(MONTH, DATEFROMPARTS(li.Nam, li.Thang, li.Ngay), @ngayTinh) = -10, l.VietTat + '_' + CONVERT(VARCHAR(10), li.Ngay) + '_' + CONVERT(VARCHAR(10), li.LichKiemTraID), '') as cot21
		from [dbo].[LichKiemTra] li, [dbo].[LoaiKiemTra] l, [dbo].[DIC_DEPARTMENT] p
		where li.LoaiKiemTraID = l.LoaiKiemTraID and p.DepartmentID = li.DepartmentID
		and DATEFROMPARTS(li.Nam, li.Thang, li.Ngay) between @ngayMin and @ngayMax) as kq
		GROUP BY kq.DepartmentID, kq.Description
	end
GO
-------------------------------------------------------------------------------------
-------lấy thông tin điều động bậc thang theo chức danh
-------đầu vào là: mã chức danh @posID, ngày cần lấy thông tin @ngayTinh, tên viết tắt của chức danh đó @posName, ví dụ nếu @posID =20 thì @posNam ='CAPT'
USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_T_LayDieuDongBacThangTheoChucDanh]    Script Date: 4/26/2024 8:52:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[sp_T_LayDieuDongBacThangTheoChucDanh]
@posID int, @ngayTinh smalldatetime, @posName nvarchar(20)
as
	begin
		--tiếp theo là lấy điều động bậc thang của người hiện tại
		--description là tên tàu viết tắt_tên chức danh viết tắt
		--format kết quả là Tên người_ngày lên tàu_số tháng đi tàu_idDieu dong_0 nếu là người hiện tại, 1 là người thay thế
		select 'DDBACTHANG' as Loai, A.DepartmentID, A.Description, 
MAX(CASE WHEN B.cot1 is not null AND B.cot1 <>'' THEN B.cot1 ELSE A.cot1 END) AS cot1,
MAX(CASE WHEN B.cot2 is not null AND B.cot2 <>'' THEN B.cot2 ELSE A.cot2 END) AS cot2,
MAX(CASE WHEN B.cot3 is not null AND B.cot3 <>'' THEN B.cot3 ELSE A.cot3 END) AS cot3,
MAX(CASE WHEN B.cot4 is not null AND B.cot4 <>'' THEN B.cot4 ELSE A.cot4 END) AS cot4,
MAX(CASE WHEN B.cot5 is not null AND B.cot5 <>'' THEN B.cot5 ELSE A.cot5 END) AS cot5,
MAX(CASE WHEN B.cot6 is not null AND B.cot6 <>'' THEN B.cot6 ELSE A.cot6 END) AS cot6,
MAX(CASE WHEN B.cot7 is not null AND B.cot7 <>'' THEN B.cot7 ELSE A.cot7 END) AS cot7,
MAX(CASE WHEN B.cot8 is not null AND B.cot8 <>'' THEN B.cot8 ELSE A.cot8 END) AS cot8,
MAX(CASE WHEN B.cot9 is not null AND B.cot9 <>'' THEN B.cot9 ELSE A.cot9 END) AS cot9,
MAX(CASE WHEN B.cot10 is not null AND B.cot10 <>'' THEN B.cot10 ELSE A.cot10 END) AS cot10,
MAX(CASE WHEN B.cot11 is not null AND B.cot11 <>'' THEN B.cot11 ELSE A.cot11 END) AS cot11,
MAX(CASE WHEN B.cot12 is not null AND B.cot12 <>'' THEN B.cot12 ELSE A.cot12 END) AS cot12,
MAX(CASE WHEN B.cot13 is not null AND B.cot13 <>'' THEN B.cot13 ELSE A.cot13 END) AS cot13,
MAX(CASE WHEN B.cot14 is not null AND B.cot14 <>'' THEN B.cot14 ELSE A.cot14 END) AS cot14,
MAX(CASE WHEN B.cot15 is not null AND B.cot15 <>'' THEN B.cot15 ELSE A.cot15 END) AS cot15,
MAX(CASE WHEN B.cot16 is not null AND B.cot16 <>'' THEN B.cot16 ELSE A.cot16 END) AS cot16,
MAX(CASE WHEN B.cot17 is not null AND B.cot17 <>'' THEN B.cot17 ELSE A.cot17 END) AS cot17,
MAX(CASE WHEN B.cot18 is not null AND B.cot18 <>'' THEN B.cot18 ELSE A.cot18 END) AS cot18,
MAX(CASE WHEN B.cot19 is not null AND B.cot19 <>'' THEN B.cot19 ELSE A.cot19 END) AS cot19,
MAX(CASE WHEN B.cot20 is not null AND B.cot20 <>'' THEN B.cot20 ELSE A.cot20 END) AS cot20,
MAX(CASE WHEN B.cot21 is not null AND B.cot21 <>'' THEN B.cot21 ELSE A.cot21 END) AS cot21
		from (select  dd.DepartmentID as DepartmentID, (dd.Tau + '_' + CONVERT(VARCHAR(2), @posID) + '_' + @posName) as Description, dd.DieuDongBacThangID,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 10, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot1,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 9, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot2,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 8, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot3,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 7, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot4,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 6, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot5,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 5, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot6,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 4, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot7,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 3, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot8,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 2, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot9,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 1, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot10,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = 0, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot11,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -1, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot12,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -2, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot13,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -3, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot14,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -4, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot15,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -5, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot16,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -6, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot17,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -7, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot18,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -8, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot19,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -9, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot20,
			IIF(DATEDIFF(MONTH, dd.NgayLenTau, @ngayTinh) = -10, dd.NguoiDangDi + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayLenTau)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTau) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_0', '') as cot21
		from [dbo].[DieuDongBacThang] dd
		where dd.NgayLenTau is not null and dd.PositionID= @posID) as A
		LEFT JOIN 
			(select  dd.DepartmentID as DepartmentID, (dd.Tau + '_' + CONVERT(VARCHAR(2), @posID) + '_'+ @posName) as Description, dd.DieuDongBacThangID,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 10, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot1,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 9, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot2,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 8, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot3,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 7, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot4,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 6, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot5,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 5, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot6,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 4, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot7,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 3, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot8,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 2, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot9,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 1, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot10,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = 0, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot11,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -1, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot12,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -2, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot13,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -3, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot14,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -4, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot15,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -5, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot16,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -6, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot17,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -7, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot18,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -8, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot19,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -9, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot20,
			IIF(DATEDIFF(MONTH, dd.NgayThayThe, @ngayTinh) = -10, dd.NguoiThayThe + '_' + CONVERT(VARCHAR(10), DAY(dd.NgayThayThe)) + '_' + CONVERT(VARCHAR(10), dd.ThangDiTauThayThe) + '_' + CONVERT(VARCHAR(10), dd.DieuDongBacThangID) + '_1', '') as cot21
		from [dbo].[DieuDongBacThang] dd
		where dd.NgayThayThe is not null and dd.NguoiThayThe is not null and dd.NguoiThayThe <>'' and dd.PositionID= @posID) as B ON A.DieuDongBacThangID = B.DieuDongBacThangID 
		GROUP BY A.DepartmentID, A.Description
	end
----------------------------------------------------------
------------store dùng để lấy người thay thế cho 1 chức danh cụ thể
ALTER PROCEDURE  [dbo].[sp_T_LayNguoiThayThe]
	@NguoiThayTheID int,
	@posID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
 
declare @ngayBaoCao as smalldatetime
select @ngayBaoCao = GETDATE()

SELECT v.*, abs(DATEDIFF(day, v.NgayXuongTau, @ngayBaoCao)) As SoNgay, 
dbo.fc_GetYMD(v.NgayXuongTau, @ngayBaoCao) as TGDuTru, 
dbo.fc_QuaTrinhDiTau(v.EmployeeID) as QTDT, 
'' as ThanNhan,
e.SSDD as EmSSDD, e.Note as NoteEm
from viewHRM_EMPLOYMENTHISTORY v, HRM_EMPLOYEE e
where v.EmployeeID <> @NguoiThayTheID 
and dbo.fc_GetParentDepartment(v.EmployeeID) = 17  
and v.StatusID =1
and v.EmployeeID = e.EmployeeID
and v.PositionID = @posID
and not exists (select * from DieuDongBacThang where NguoiThayTheID = v.EmployeeID)
END


