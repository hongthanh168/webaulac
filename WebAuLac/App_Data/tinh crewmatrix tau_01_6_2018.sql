﻿---------------------hàm tính crewMatrix tàu---------------------
CREATE procedure [dbo].[sp_T_CrewMatrixTau]
(
	@idTau int,
	@idPlan int,
	@idOn int,
	@idOff int,
	@tieuChuan int
)
as
	begin
		---tạo 1 bảng thông tin trả về, mỗi 1 trường dữ liệu trong bảng có định dạng
		-- Giá trị 0 (không đạt) hoặc 1 (đạt), giá trị crewMatrix
		declare @tempTable TABLE (
									[KetQua] bit,
									[ThuyenTruongID] int, 
									[DaiPhoID] int, 
									[Pho2ID] int, 
									[Pho3ID] int, 
									[MayTruongID] int, 
									[May2ID] int, 
									[May3ID] int, 
									[May4ID] int, 
									[WithOperator_QuanLyBoong] nvarchar(10) NULL,
									[InRank_QuanLyBoong] nvarchar(10) NULL,
									[ThisTypeOfTanker_QuanLyBoong] nvarchar(10) NULL,
									[AllTypeOfTanker_QuanLyBoong] nvarchar(10) NULL,
									[WithOperator_ThuyenTruong] nvarchar(10) NULL,
									[InRank_ThuyenTruong] nvarchar(10) NULL,
									[ThisTypeOfTanker_ThuyenTruong] nvarchar(10) NULL,
									[AllTypeOfTanker_ThuyenTruong] nvarchar(10) NULL,
									[WithOperator_DaiPho] nvarchar(10) NULL,
									[InRank_DaiPho] nvarchar(10) NULL,
									[ThisTypeOfTanker_DaiPho] nvarchar(10) NULL,
									[AllTypeOfTanker_DaiPho] nvarchar(10) NULL,
									[WithOperator_QuanLyMay] nvarchar(10) NULL,
									[InRank_QuanLyMay] nvarchar(10) NULL,
									[ThisTypeOfTanker_QuanLyMay] nvarchar(10) NULL,
									[AllTypeOfTanker_QuanLyMay] nvarchar(10) NULL,
									[WithOperator_MayTruong] nvarchar(10) NULL,
									[InRank_MayTruong] nvarchar(10) NULL,
									[ThisTypeOfTanker_MayTruong] nvarchar(10) NULL,
									[AllTypeOfTanker_MayTruong] nvarchar(10) NULL,
									[WithOperator_May2] nvarchar(10) NULL,
									[InRank_May2] nvarchar(10) NULL,
									[ThisTypeOfTanker_May2] nvarchar(10) NULL,
									[AllTypeOfTanker_May2] nvarchar(10) NULL,
									[WithOperator_VanHanhBoong] nvarchar(10) NULL,
									[InRank_VanHanhBoong] nvarchar(10) NULL,
									[ThisTypeOfTanker_VanHanhBoong] nvarchar(10) NULL,
									[AllTypeOfTanker_VanHanhBoong] nvarchar(10) NULL,
									[WithOperator_VanHanhMay] nvarchar(10) NULL,
									[InRank_VanHanhMay] nvarchar(10) NULL,
									[ThisTypeTanker_VanHanhMay] nvarchar(10) NULL,
									[AllTypeOfTanker_VanHanhMay] nvarchar(10) NULL,
									[WatchKeeping_ThuyenTruong] nvarchar(10),
									[WatchKeeping_DaiPho] nvarchar(10),
									[WatchKeeping_MayTruong] nvarchar(10),
									[WatchKeeping_May2] nvarchar(10)
								)
		--lấy ra danh sách sỹ quan của tàu đó
		--danh sách mã của các chức danh tính crewmatrix
		--[quản lý boong]
		--Thuyền trưởng: 20
		--Đại phó:21
		--[vận hành boong]
		--Phó 2: 22
		--Phó 3: 23
		--[quản lý máy]
		--Máy trưởng: 24
		--Máy 2: 25
		--[vận hành máy]
		--Máy 3: 26
		--Máy 4: 27
		declare @idThuyenTruong int, @idDaiPho int, @idPho2 int, @idPho3 int, @idMayTruong int, @idMay2 int, @idMay3 int, @idMay4 int
		set @idThuyenTruong =0
		set @idDaiPho=0
		set @idMay2=0
		set @idMay3=0
		set @idMay4 =0
		set @idMayTruong=0
		set @idPho2 =0
		set @idPho3 =0
		declare @chucVuOn int, @sChucVuOn nvarchar(50)
		set @chucVuOn =0 -- giá trị mặc định
		--lấy ra chức vụ của người lên tàu và rời tàu
		select @sChucVuOn = dbo.fc_GetPosition(@idOn)
		--lấy ra id chức vụ của người lên tàu và rời tàu
		select @chucVuOn = PositionID
		from DIC_POSITION
		where PositionName = @sChucVuOn

		--lấy ra người thuyền trưởng của tàu đó - mã 20
		--trước tiên là lấy từ kế hoạch
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =20))
			begin
				select @idThuyenTruong = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =20 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 20
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idThuyenTruong = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =20
			end

		--lấy ra người đại phó của tàu đó - mã 21
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =21))
			begin
				select @idDaiPho = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =21 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 21
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idDaiPho = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =21
			end

		--lấy ra người phó 2 của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =22))
			begin
				select @idPho2 = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =22 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 22
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idPho2= EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =22
			end
		--lấy ra người phó 3 của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =23))
			begin
				select @idPho3 = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =23 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 23
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idPho3 = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =23
			end
		--lấy ra máy trưởng của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =24))
			begin
				select @idMayTruong = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =24 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 24
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idMayTruong = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =24
			end
		--lấy ra người máy 2 của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =25))
			begin
				select @idMay2 = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =25 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 25
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idMay2 = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =25
			end
		--lấy ra người máy 3 của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =26))
			begin
				select @idMay3 = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =26 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 26
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idMay3 = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =26
			end
		--lấy ra người phó 2 của tàu đó
		if (@idPlan>0 and exists (select * from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =27))
			begin
				select @idMay4 = CrewOnID
				from HRM_DETAILPLAN 
				where CrewOffDepartmentID =@idTau and PlanID = @idPlan and OnPositionID =27 
			end
		--nếu không có trong kế hoạch thì lấy ra trong EmploymentHistory
		else if exists (select * from viewHRM_EMPLOYMENTHISTORY 
						where DepartmentID = @idTau and PositionID = 27
						and EmployeeID<>@idOff
						and EmployeeID not in (select CrewOffID from HRM_DETAILPLAN where CrewOffDepartmentID =@idTau and PlanID = @idPlan ))
			begin
				select @idMay4 = EmployeeID
				from viewHRM_EMPLOYMENTHISTORY
				where DepartmentID = @idTau and PositionID =27
			end
		--xét với người sắp lên tàu
		if (@chucVuOn=20)
			begin 
				set @idThuyenTruong = @idOn
			end
		else if (@chucVuOn =21)
			begin
				set @idDaiPho = @idOn
			end
		else if (@chucVuOn =22)
			begin
				set @idPho2 = @idOn
			end
		else if (@chucVuOn =23)
			begin
				set @idPho3 = @idOn
			end
		else if (@chucVuOn =24)
			begin
				set @idMayTruong = @idOn
			end
		else if (@chucVuOn =25)
			begin
				set @idMay2 = @idOn
			end
		else if (@chucVuOn =26)
			begin
				set @idMay3 = @idOn
			end
		else if (@chucVuOn =27)
			begin
				set @idMay4 = @idOn
			end
			
		---bắt đầu tính các chỉ số crewmatrix
		--khai báo các chỉ các crewMatrix của cái tiêu chuẩn đó
		declare @withOperator_quanlyboong_chuan decimal(4,2), @inRank_quanlyboong_chuan decimal(4,2), @thisType_quanlyboong_chuan decimal(4,2), @allType_quanlyboong_chuan decimal(4,2),
				@withOperator_vanhanhboong_chuan decimal(4,2), @inRank_vanhanhboong_chuan decimal(4,2), @thisType_vanhanhboong_chuan decimal(4,2), @allType_vanhanhboong_chuan decimal(4,2),
				@withOperator_quanlymay_chuan decimal(4,2), @inRank_quanlymay_chuan decimal(4,2), @thisType_quanlymay_chuan decimal(4,2), @allType_quanlymay_chuan decimal(4,2),
				@withOperator_vanhanhmay_chuan decimal(4,2), @inRank_vanhanhmay_chuan decimal(4,2), @thisType_vanhanhmay_chuan decimal(4,2), @allType_vanhanhmay_chuan decimal(4,2),
				@withOperator_thuyentruong_chuan decimal(4,2), @inRank_thuyentruong_chuan decimal(4,2), @thisType_thuyentruong_chuan decimal(4,2), @allType_thuyentruong_chuan decimal(4,2),
				@withOperator_daipho_chuan decimal(4,2), @inRank_daipho_chuan decimal(4,2), @thisType_daipho_chuan decimal(4,2), @allType_daipho_chuan decimal(4,2),
				@withOperator_maytruong_chuan decimal(4,2), @inRank_maytruong_chuan decimal(4,2), @thisType_maytruong_chuan decimal(4,2), @allType_maytruong_chuan decimal(4,2),
				@withOperator_may2_chuan decimal(4,2), @inRank_may2_chuan decimal(4,2), @thisType_may2_chuan decimal(4,2), @allType_may2_chuan decimal(4,2)

		select @withOperator_quanlyboong_chuan = WithOperator_QuanLyBoong , 
			   @inRank_quanlyboong_chuan = InRank_QuanLyBoong  , 
			   @thisType_quanlyboong_chuan = ThisTypeOfTanker_QuanLyBoong , 
			   @allType_quanlyboong_chuan = AllTypeOfTanker_QuanLyBoong ,
			   @withOperator_vanhanhboong_chuan = WithOperator_vanhanhboong , 
			   @inRank_vanhanhboong_chuan = InRank_vanhanhboong , 
			   @thisType_vanhanhboong_chuan = ThisTypeOfTanker_vanhanhboong , 
			   @allType_vanhanhboong_chuan = AllTypeOfTanker_vanhanhboong ,
			   @withOperator_quanlymay_chuan = WithOperator_quanlymay , 
			   @inRank_quanlymay_chuan = InRank_quanlymay , 
			   @thisType_quanlymay_chuan = ThisTypeOfTanker_quanlymay , 
			   @allType_quanlymay_chuan = AllTypeOfTanker_quanlymay ,
			   @withOperator_vanhanhmay_chuan = WithOperator_vanhanhmay , 
			   @inRank_vanhanhmay_chuan = InRank_vanhanhmay , 
			   @thisType_vanhanhmay_chuan = ThisTypeTanker_VanHanhMay , 
			   @allType_vanhanhmay_chuan = AllTypeOfTanker_vanhanhmay ,
			   @withOperator_thuyentruong_chuan = WithOperator_thuyentruong , 
			   @inRank_thuyentruong_chuan = InRank_thuyentruong , 
			   @thisType_thuyentruong_chuan = ThisTypeOfTanker_ThuyenTruong , 
			   @allType_thuyentruong_chuan = AllTypeOfTanker_thuyentruong ,
			   @withOperator_daipho_chuan = WithOperator_daipho , 
			   @inRank_daipho_chuan = InRank_daipho , 
			   @thisType_daipho_chuan = ThisTypeOfTanker_DaiPho , 
			   @allType_daipho_chuan = AllTypeOfTanker_daipho ,
			   @withOperator_maytruong_chuan = WithOperator_maytruong , 
			   @inRank_maytruong_chuan = InRank_maytruong , 
			   @thisType_maytruong_chuan = ThisTypeOfTanker_maytruong , 
			   @allType_maytruong_chuan = AllTypeOfTanker_maytruong ,
			   @withOperator_may2_chuan = WithOperator_may2 , 
			   @inRank_may2_chuan = InRank_may2 , 
			   @thisType_may2_chuan = ThisTypeOfTanker_may2 , 
			   @allType_may2_chuan = AllTypeOfTanker_may2 
		from TieuChuanCrewMatrix
		where TieuChuanID = @tieuChuan

		--tính crewMatrix
		declare @withOperator_quanlyboong decimal(4,2), @inRank_quanlyboong decimal(4,2), @thisType_quanlyboong decimal(4,2), @allType_quanlyboong decimal(4,2),
		@withOperator_vanhanhboong decimal(4,2), @inRank_vanhanhboong decimal(4,2), @thisType_vanhanhboong decimal(4,2), @allType_vanhanhboong decimal(4,2),
		@withOperator_quanlymay decimal(4,2), @inRank_quanlymay decimal(4,2), @thisType_quanlymay decimal(4,2), @allType_quanlymay decimal(4,2),
		@withOperator_vanhanhmay decimal(4,2), @inRank_vanhanhmay decimal(4,2), @thisType_vanhanhmay decimal(4,2), @allType_vanhanhmay decimal(4,2),
		@withOperator_thuyentruong decimal(4,2), @inRank_thuyentruong decimal(4,2), @thisType_thuyentruong decimal(4,2), @allType_thuyentruong decimal(4,2),
		@withOperator_daipho decimal(4,2), @inRank_daipho decimal(4,2), @thisType_daipho decimal(4,2), @allType_daipho decimal(4,2),
		@withOperator_maytruong decimal(4,2), @inRank_maytruong decimal(4,2), @thisType_maytruong decimal(4,2), @allType_maytruong decimal(4,2),
		@withOperator_may2 decimal(4,2), @inRank_may2 decimal(4,2), @thisType_may2 decimal(4,2), @allType_may2 decimal(4,2),
		@watchKeeping_thuyentruong decimal(4,2),
		@watchKeeping_daipho decimal(4,2),
		@watchKeeping_maytruong decimal(4,2),
		@watchKeeping_may2 decimal(4,2)

		set @withOperator_thuyentruong = cast(dbo.fc_T_CrewMatrix_WithOperator(@idThuyenTruong) as decimal(8,2) )/365
		set @inRank_thuyentruong = cast(dbo.fc_T_CrewMatrix_InRank(20,@idThuyenTruong) as decimal(8,2))/365
		set @thisType_thuyentruong = cast(dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idThuyenTruong) as decimal(8,2))/365
		set @allType_thuyentruong = cast(dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idThuyenTruong) as decimal(8,2))/365

		set @withOperator_daiPho = cast(dbo.fc_T_CrewMatrix_WithOperator(@iddaiPho) as decimal(8,2))/365
		set @inRank_daiPho = cast(dbo.fc_T_CrewMatrix_InRank(21,@iddaiPho) as decimal(8,2))/365
		set @thisType_daiPho = cast(dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@iddaiPho) as decimal(8,2))/365
		set @allType_daiPho = cast(dbo.fc_T_CrewMatrix_AllTypeOfTanker(@iddaiPho) as decimal(8,2))/365

		set @withOperator_mayTruong = cast(dbo.fc_T_CrewMatrix_WithOperator(@idmayTruong) as decimal(8,2))/365
		set @inRank_mayTruong = cast(dbo.fc_T_CrewMatrix_InRank(24,@idmayTruong) as decimal(8,2))/365
		set @thisType_mayTruong = cast(dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idmayTruong) as decimal(8,2))/365
		set @allType_mayTruong = cast(dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idmayTruong) as decimal(8,2))/365

		set @withOperator_may2 = cast(dbo.fc_T_CrewMatrix_WithOperator(@idmay2) as decimal(8,2))/365
		set @inRank_may2 = cast(dbo.fc_T_CrewMatrix_InRank(25,@idmay2) as decimal(8,2))/365
		set @thisType_may2 = cast(dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idmay2) as decimal(8,2))/365
		set @allType_may2 = cast(dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idmay2) as decimal(8,2))/365
		
		set @withOperator_quanlyboong = @withOperator_thuyentruong + @withOperator_daipho
		set @withOperator_vanhanhboong = cast((dbo.fc_T_CrewMatrix_WithOperator(@idPho2) + dbo.fc_T_CrewMatrix_WithOperator(@idPho3)) as decimal(8,2))/365
		set @withOperator_quanlymay = @withOperator_maytruong + @withOperator_may2
		set @withOperator_vanhanhmay = cast((dbo.fc_T_CrewMatrix_WithOperator(@idMay4) + dbo.fc_T_CrewMatrix_WithOperator(@idMay3)) as decimal(8,2))/365

		set @inRank_quanlyboong = @inRank_thuyentruong +@inRank_daipho
		set @inRank_vanhanhboong = cast((dbo.fc_T_CrewMatrix_InRank(22,@idPho2) + dbo.fc_T_CrewMatrix_InRank(23,@idPho3)) as decimal(8,2))/365
		set @inRank_quanlymay = @inRank_maytruong + @inRank_may2 
		set @inRank_vanhanhmay = cast((dbo.fc_T_CrewMatrix_InRank(26,@idMay3) + dbo.fc_T_CrewMatrix_InRank(27,@idMay4)) as decimal(8,2))/365

		set @thisType_quanlyboong = @thisType_thuyentruong + @thisType_daipho
		set @thisType_vanhanhboong = cast((dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idPho2) + dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idPho3)) as decimal(8,2))/365
		set @thisType_quanlymay = @thisType_maytruong + @thisType_may2
		set @thisType_vanhanhmay = cast((dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idMay4) + dbo.fc_T_CrewMatrix_ThisTypeOfTanker(@idMay3)) as decimal(8,2))/365

		set @allType_quanlyboong = @allType_thuyentruong + @allType_daipho
		set @allType_vanhanhboong = cast((dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idPho2) + dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idPho3)) as decimal(8,2))/365
		set @allType_quanlymay = @allType_maytruong + @allType_may2
		set @allType_vanhanhmay = cast((dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idMay4) + dbo.fc_T_CrewMatrix_AllTypeOfTanker(@idMay3)) as decimal(8,2))/365
		
		set @watchKeeping_thuyentruong = cast(dbo.fc_T_CrewMatrix_WatchKeeping(@idThuyenTruong) as decimal(8,2))/365
		set @watchKeeping_daipho = cast(dbo.fc_T_CrewMatrix_WatchKeeping(@idDaiPho) as decimal(8,2))/365
		set @watchKeeping_maytruong = cast(dbo.fc_T_CrewMatrix_WatchKeeping(@idMayTruong) as decimal(8,2))/365
		set @watchKeeping_may2 = cast(dbo.fc_T_CrewMatrix_WatchKeeping(@idMay2) as decimal(8,2))/365

		declare @kq bit, 
		@swithOperator_quanlyboong nvarchar(10), @sinRank_quanlyboong nvarchar(10), @sthisType_quanlyboong nvarchar(10), @sallType_quanlyboong nvarchar(10),
		@swithOperator_vanhanhboong nvarchar(10), @sinRank_vanhanhboong nvarchar(10), @sthisType_vanhanhboong nvarchar(10), @sallType_vanhanhboong nvarchar(10),
		@swithOperator_quanlymay nvarchar(10), @sinRank_quanlymay nvarchar(10), @sthisType_quanlymay nvarchar(10), @sallType_quanlymay nvarchar(10),
		@swithOperator_vanhanhmay nvarchar(10), @sinRank_vanhanhmay nvarchar(10), @sthisType_vanhanhmay nvarchar(10), @sallType_vanhanhmay nvarchar(10),
		@swithOperator_thuyentruong nvarchar(10), @sinRank_thuyentruong nvarchar(10), @sthisType_thuyentruong nvarchar(10), @sallType_thuyentruong nvarchar(10),
		@swithOperator_daipho nvarchar(10), @sinRank_daipho nvarchar(10), @sthisType_daipho nvarchar(10), @sallType_daipho nvarchar(10),
		@swithOperator_maytruong nvarchar(10), @sinRank_maytruong nvarchar(10), @sthisType_maytruong nvarchar(10), @sallType_maytruong nvarchar(10),
		@swithOperator_may2 nvarchar(10), @sinRank_may2 nvarchar(10), @sthisType_may2 nvarchar(10), @sallType_may2 nvarchar(10)
		
		set @kq =1

		--crewMatrix quản lý boong
		if (@withOperator_quanlyboong>@withOperator_quanlyboong_chuan)
			begin
				set @swithOperator_quanlyboong = '1_' + cast(@withOperator_quanlyboong as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_quanlyboong = '0_' + cast( @withOperator_quanlyboong as varchar)
			end

		if (@inRank_quanlyboong>@inRank_quanlyboong_chuan)
			begin
				set @sinRank_quanlyboong = '1_' + cast( @inRank_quanlyboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_quanlyboong = '0_' + cast( @inRank_quanlyboong as varchar)
			end

		if (@thisType_quanlyboong>@thisType_quanlyboong_chuan)
			begin
				set @sthisType_quanlyboong = '1_' + cast( @thisType_quanlyboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_quanlyboong = '0_' + cast( @thisType_quanlyboong as varchar)
			end

		if (@allType_quanlyboong>@allType_quanlyboong_chuan)
			begin
				set @sallType_quanlyboong = '1_' + cast( @allType_quanlyboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_quanlyboong = '0_' + cast( @allType_quanlyboong as varchar)
			end
		--crewMatrix vận hành boong
		if (@withOperator_vanhanhboong>@withOperator_vanhanhboong_chuan)
			begin
				set @swithOperator_vanhanhboong = '1_' + cast( @withOperator_vanhanhboong as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_vanhanhboong = '0_' + cast( @withOperator_vanhanhboong as varchar)
			end

		if (@inRank_vanhanhboong>@inRank_vanhanhboong_chuan)
			begin
				set @sinRank_vanhanhboong = '1_' + cast( @inRank_vanhanhboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_vanhanhboong = '0_' + cast( @inRank_vanhanhboong as varchar)
			end

		if (@thisType_vanhanhboong>@thisType_vanhanhboong_chuan)
			begin
				set @sthisType_vanhanhboong = '1_' + cast( @thisType_vanhanhboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_vanhanhboong = '0_' + cast( @thisType_vanhanhboong as varchar)
			end

		if (@allType_vanhanhboong>@allType_vanhanhboong_chuan)
			begin
				set @sallType_vanhanhboong = '1_' + cast( @allType_vanhanhboong as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_vanhanhboong = '0_' + cast( @allType_vanhanhboong as varchar)
			end
		
		--crewMatrix quản lý máy
		if (@withOperator_quanlymay>@withOperator_quanlymay_chuan)
			begin
				set @swithOperator_quanlymay = '1_' + cast( @withOperator_quanlymay as varchar )
			end
		else
			begin
				set @kq =0
				set @swithOperator_quanlymay = '0_' + cast( @withOperator_quanlymay as varchar)
			end

		if (@inRank_quanlymay>@inRank_quanlymay_chuan)
			begin
				set @sinRank_quanlymay = '1_' + cast( @inRank_quanlymay as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_quanlymay = '0_' + cast( @inRank_quanlymay as varchar)
			end

		if (@thisType_quanlymay>@thisType_quanlymay_chuan)
			begin
				set @sthisType_quanlymay = '1_' + cast( @thisType_quanlymay as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_quanlymay = '0_' + cast( @thisType_quanlymay as varchar)
			end

		if (@allType_quanlymay>@allType_quanlymay_chuan)
			begin
				set @sallType_quanlymay = '1_' + cast( @allType_quanlymay as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_quanlymay = '0_' + cast( @allType_quanlymay as varchar)
			end

		--crewMatrix vận hành máy
		if (@withOperator_vanhanhmay>@withOperator_vanhanhmay_chuan)
			begin
				set @swithOperator_vanhanhmay = '1_' + cast( @withOperator_vanhanhmay as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_vanhanhmay = '0_' + cast( @withOperator_vanhanhmay as varchar)
			end

		if (@inRank_vanhanhmay>@inRank_vanhanhmay_chuan)
			begin
				set @sinRank_vanhanhmay = '1_' + cast( @inRank_vanhanhmay as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_vanhanhmay = '0_' + cast( @inRank_vanhanhmay as varchar)
			end

		if (@thisType_vanhanhmay>@thisType_vanhanhmay_chuan)
			begin
				set @sthisType_vanhanhmay = '1_' + cast( @thisType_vanhanhmay as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_vanhanhmay = '0_' + cast( @thisType_vanhanhmay as varchar)
			end

		if (@allType_vanhanhmay>@allType_vanhanhmay_chuan)
			begin
				set @sallType_vanhanhmay = '1_' + cast( @allType_vanhanhmay as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_vanhanhmay = '0_' + cast( @allType_vanhanhmay as varchar)
			end

		--crewMatrix thuyền trưởng
		if (@withOperator_thuyentruong>@withOperator_thuyentruong_chuan)
			begin
				set @swithOperator_thuyentruong = '1_' + cast( @withOperator_thuyentruong as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_thuyentruong = '0_' + cast( @withOperator_thuyentruong as varchar)
			end

		if (@inRank_thuyentruong>@inRank_thuyentruong_chuan)
			begin
				set @sinRank_thuyentruong = '1_' + cast( @inRank_thuyentruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_thuyentruong = '0_' + cast( @inRank_thuyentruong as varchar)
			end

		if (@thisType_thuyentruong>@thisType_thuyentruong_chuan)
			begin
				set @sthisType_thuyentruong = '1_' + cast( @thisType_thuyentruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_thuyentruong = '0_' + cast( @thisType_thuyentruong as varchar)
			end

		if (@allType_thuyentruong>@allType_thuyentruong_chuan)
			begin
				set @sallType_thuyentruong = '1_' + cast( @allType_thuyentruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_thuyentruong = '0_' + cast( @allType_thuyentruong as varchar)
			end

		--crewMatrix đại phó
		if (@withOperator_daipho>@withOperator_daipho_chuan)
			begin
				set @swithOperator_daipho = '1_' + cast( @withOperator_daipho as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_daipho = '0_' + cast( @withOperator_daipho as varchar)
			end

		if (@inRank_daipho>@inRank_daipho_chuan)
			begin
				set @sinRank_daipho = '1_' + cast( @inRank_daipho as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_daipho = '0_' + cast( @inRank_daipho as varchar) 
			end

		if (@thisType_daipho>@thisType_daipho_chuan)
			begin
				set @sthisType_daipho = '1_' + cast( @thisType_daipho as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_daipho = '0_' + cast( @thisType_daipho as varchar)
			end

		if (@allType_daipho>@allType_daipho_chuan)
			begin
				set @sallType_daipho = '1_' + cast( @allType_daipho as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_daipho = '0_' + cast( @allType_daipho as varchar)
			end

		--crewMatrix máy trưởng
		if (@withOperator_maytruong>@withOperator_maytruong_chuan)
			begin
				set @swithOperator_maytruong = '1_' + cast( @withOperator_maytruong as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_maytruong = '0_' + cast( @withOperator_maytruong as varchar)
			end

		if (@inRank_maytruong>@inRank_maytruong_chuan)
			begin
				set @sinRank_maytruong = '1_' + cast( @inRank_maytruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_maytruong = '0_' + cast( @inRank_maytruong as varchar)
			end

		if (@thisType_maytruong>@thisType_maytruong_chuan)
			begin
				set @sthisType_maytruong = '1_' + cast( @thisType_maytruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_maytruong = '0_' + cast( @thisType_maytruong as varchar)
			end

		if (@allType_maytruong>@allType_maytruong_chuan)
			begin
				set @sallType_maytruong = '1_' + cast( @allType_maytruong as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_maytruong = '0_' + cast( @allType_maytruong as varchar)
			end

		--crewMatrix máy 2
		if (@withOperator_may2>@withOperator_may2_chuan)
			begin
				set @swithOperator_may2 = '1_' + cast( @withOperator_may2 as varchar)
			end
		else
			begin
				set @kq =0
				set @swithOperator_may2 = '0_' + cast( @withOperator_may2 as varchar)
			end

		if (@inRank_may2>@inRank_may2_chuan)
			begin
				set @sinRank_may2 = '1_' + cast( @inRank_may2 as varchar)
			end
		else
			begin
				set @kq =0
				set @sinRank_may2 = '0_' + cast( @inRank_may2 as varchar)
			end

		if (@thisType_may2>@thisType_may2_chuan)
			begin
				set @sthisType_may2 = '1_' + cast( @thisType_may2 as varchar)
			end
		else
			begin
				set @kq =0
				set @sthisType_may2 = '0_' + cast( @thisType_may2 as varchar)
			end

		if (@allType_may2>@allType_may2_chuan)
			begin
				set @sallType_may2 = '1_' + cast( @allType_may2 as varchar)
			end
		else
			begin
				set @kq =0
				set @sallType_may2 = '0_' + cast(@allType_may2 as varchar)
			end

	 --insert vào
	 insert into @tempTable values(	@kq,
									@idThuyenTruong,
									@idDaiPho,
									@idPho2,
									@idPho3,
									@idMayTruong,
									@idMay2,
									@idMay3,
									@idMay4,
									@sWithOperator_QuanLyBoong ,
									@sInRank_QuanLyBoong ,
									@sThisType_QuanLyBoong ,
									@sAllType_QuanLyBoong ,
									@sWithOperator_ThuyenTruong ,
									@sInRank_ThuyenTruong ,
									@sThisType_ThuyenTruong ,
									@sAllType_ThuyenTruong ,
									@sWithOperator_DaiPho ,
									@sInRank_DaiPho ,
									@sThisType_DaiPho ,
									@sAllType_DaiPho ,
									@sWithOperator_QuanLyMay ,
									@sInRank_QuanLyMay ,
									@sThisType_QuanLyMay ,
									@sAllType_QuanLyMay ,
									@sWithOperator_MayTruong ,
									@sInRank_MayTruong ,
									@sThisType_MayTruong ,
									@sAllType_MayTruong ,
									@sWithOperator_May2 ,
									@sInRank_May2,
									@sThisType_May2 ,
									@sAllType_May2 ,
									@sWithOperator_VanHanhBoong ,
									@sInRank_VanHanhBoong ,
									@sThisType_VanHanhBoong ,
									@sAllType_VanHanhBoong ,
									@sWithOperator_VanHanhMay ,
									@sInRank_VanHanhMay ,
									@sThisType_VanHanhMay ,
									@sAllType_VanHanhMay,
									cast (@watchKeeping_thuyentruong as varchar),
									cast(@watchKeeping_daipho as varchar),
									cast(@watchKeeping_maytruong as varchar),
									cast(@watchKeeping_may2 as varchar)
						)
	 select * from @tempTable
	end

GO


