USE [AuLac]
GO

/****** Object:  StoredProcedure [dbo].[sp_ThoiGianLamViecTaiCongTy]    Script Date: 26/11/2018 11:01:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[sp_ThoiGianLamViecTaiCongTy]
	-- Add the parameters for the stored procedure here	
	@employeeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
 
	declare @date3 smalldatetime
	declare @date smalldatetime		-- Ngay Hien Tai
	declare @date2 smalldatetime	-- Ngay Hop Dong
	declare @ngayHopDong smalldatetime

	set @date = GETDATE()

	select @date2 = min(ContractDate) from HRM_CONTRACTHISTORY where EmployeeID = @employeeID
	set @ngayHopDong = @date2
	
    Declare @month int,@year int,@day int

	if (@date2 is null)
		select @employeeID as EmployeeID, @date2 as NgayHopDong, '' as ThoiGianTaiCongTy
	else
	begin

		 if @date > @date2
		 begin
		 set @date3=@date2
		 set @date2=@date
		 set @date=@date3
		 end



		SELECT @month=datediff (MONTH,@date,@date2)

		if dateadd(month,@month,@date) >@date2
		begin
		set @month=@month-1
		end
		set @day=DATEDIFF(day,dateadd(month,@month,@date),@date2)

		set @year=@month/12
		set @month=@month % 12

		select @employeeID as EmployeeID, @ngayHopDong as NgayHopDong, ((case when @year=0 then '' else convert(varchar(50),@year ) + 'y' end)
		+ (case when @month=0 then '' else ' ' + convert(varchar(50),@month ) + 'm' end)
		+ (case when @day=0 then '' else ' ' + convert(varchar(50),@day ) + 'd' end)) as ThoiGianTaiCongTy
	end

END
GO


CREATE VIEW [dbo].[viewNhanVien]
AS
SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.CardNo, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.Alias, dbo.HRM_EMPLOYEE.Sex, 
                  dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, dbo.HRM_EMPLOYEE.MainAddress, dbo.HRM_EMPLOYEE.ContactAddress, dbo.HRM_EMPLOYEE.CellPhone, dbo.HRM_EMPLOYEE.HomePhone, 
                  dbo.HRM_EMPLOYEE.Email, dbo.HRM_EMPLOYEE.Skype, dbo.HRM_EMPLOYEE.Yahoo, dbo.HRM_EMPLOYEE.Facebook, dbo.HRM_EMPLOYEE.IDCard, dbo.HRM_EMPLOYEE.IDCardDate, dbo.HRM_EMPLOYEE.IDCardPlace, 
                  dbo.HRM_EMPLOYEE.TaxNo, dbo.HRM_EMPLOYEE.BankCode, dbo.HRM_EMPLOYEE.BankName, dbo.HRM_EMPLOYEE.InsuranceCode, dbo.HRM_EMPLOYEE.InsuranceDate, dbo.HRM_EMPLOYEE.Photo, 
                  dbo.HRM_EMPLOYEE.EducationID, dbo.HRM_EMPLOYEE.DegreeID, dbo.HRM_EMPLOYEE.EthnicID, dbo.HRM_EMPLOYEE.ReligionID, dbo.HRM_EMPLOYEE.NationalityID, dbo.HRM_EMPLOYEE.Department_PositionID, 
                  dbo.HRM_EMPLOYEE.StatusID, dbo.HRM_EMPLOYEE.Origin, dbo.HRM_EMPLOYEE.SchoolID, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.Height, dbo.HRM_EMPLOYEE.Weight, dbo.HRM_EMPLOYEE.BloodType, 
                  dbo.HRM_EMPLOYEE.MarriageStatus, dbo.HRM_EMPLOYEE.KhuVucID, dbo.HRM_EMPLOYEE.Note, dbo.HRM_EMPLOYEE.QuanHeID, dbo.HRM_EMPLOYEE.LoaiQuanHe, dbo.HRM_EMPLOYEE.DiaChiTiengAnh, 
                  dbo.HRM_EMPLOYEE.SSDD, dbo.HRM_EMPLOYEE.GhiChuSSDD, dbo.HRM_EMPLOYEE.SoNguoiPhuThuoc, dbo.HRM_EMPLOYEE.HeDaoTaoID, dbo.HRM_EMPLOYEE.TrinhDoAnhVanID, dbo.HRM_EMPLOYEE.TrinhDoViTinhID, 
                  dbo.HRM_EMPLOYEE.ThoiGianTotNghiep, dbo.HRM_EMPLOYEE.SDTNguoiThan, dbo.HRM_EMPLOYEE.MainAddress_Xa, dbo.HRM_EMPLOYEE.MainAddress_Huyen, dbo.HRM_EMPLOYEE.MainAddress_Tinh, 
                  dbo.HRM_EMPLOYEE.ContactAddress_Xa, dbo.HRM_EMPLOYEE.ContactAddress_Huyen, dbo.HRM_EMPLOYEE.ContactAddress_Tinh, dbo.HRM_EMPLOYEE.Origin_Xa, dbo.HRM_EMPLOYEE.Origin_Huyen, 
                  dbo.HRM_EMPLOYEE.Origin_Tinh, dbo.view_FirstEMPLOYMENTHISTORY.EmploymentHistoryID, dbo.view_FirstEMPLOYMENTHISTORY.DecisionDate, dbo.view_FirstEMPLOYMENTHISTORY.EffectiveDate, 
                  dbo.view_FirstEMPLOYMENTHISTORY.CategoryDecisionID, dbo.view_FirstEMPLOYMENTHISTORY.DepartmentID, dbo.view_FirstEMPLOYMENTHISTORY.PositionID, dbo.view_FirstEMPLOYMENTHISTORY.InternshipPosition, 
                  dbo.view_FirstEMPLOYMENTHISTORY.Note AS NoteQTCT, dbo.view_FirstEMPLOYMENTHISTORY.PerPosition, dbo.view_FirstEMPLOYMENTHISTORY.PerPlurality, dbo.view_FirstEMPLOYMENTHISTORY.NgayXuongTau, 
                  dbo.view_FirstEMPLOYMENTHISTORY.XacNhan, dbo.view_FirstEMPLOYMENTHISTORY.DecisionNo, dbo.view_FirstEMPLOYMENTHISTORY.LyDoNghiViec_ID, dbo.view_FirstEMPLOYMENTHISTORY.Salary, 
                  dbo.view_FirstEMPLOYMENTHISTORY.AllowanceSalary, dbo.view_FirstEMPLOYMENTHISTORY.Bonus, dbo.view_FirstEMPLOYMENTHISTORY.AllowanceBonus, dbo.view_FirstEMPLOYMENTHISTORY.SalaryPlurality, 
                  dbo.view_FirstEMPLOYMENTHISTORY.AllowanceSalaryPlurality, dbo.view_FirstEMPLOYMENTHISTORY.BonusPlurality, dbo.view_FirstEMPLOYMENTHISTORY.AllowanceBonusPlurality, dbo.DIC_DEPARTMENT.DepartmentName, 
                  dbo.DIC_DEPARTMENT.ParentID, ABS(DATEDIFF(day, dbo.view_FirstEMPLOYMENTHISTORY.NgayXuongTau, GETDATE())) AS SoNgay, dbo.fc_GetYMD(dbo.view_FirstEMPLOYMENTHISTORY.NgayXuongTau, GETDATE()) AS TGDT, 
                  dbo.fc_QuaTrinhDiTau(dbo.HRM_EMPLOYEE.EmployeeID) AS QTDT, dbo.DIC_EDUCATION.EducationName, dbo.fc_GetPositionByHRM_History(dbo.view_FirstEMPLOYMENTHISTORY.EmploymentHistoryID) as ChucVu
FROM     dbo.view_FirstEMPLOYMENTHISTORY INNER JOIN
                  dbo.DIC_DEPARTMENT ON dbo.view_FirstEMPLOYMENTHISTORY.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID INNER JOIN
                  dbo.HRM_EMPLOYEE ON dbo.view_FirstEMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID
WHERE  (dbo.view_FirstEMPLOYMENTHISTORY.XacNhan = 1) AND (dbo.HRM_EMPLOYEE.StatusID = 1)



GO


ALTER FUNCTION [dbo].[fc_NgayXuongTauvaVeDuTru] 
(
	-- Add the parameters for the function here
	@employeeID int
)
RETURNS DateTime
AS
BEGIN
	
	
	Declare @kq DateTime
	
	Declare @departmentID int
	Declare @parentID int
	Declare @CategoryDecisionID int
	Declare @categoryDecisionID_HienTai int

	Declare @ngayXT DateTime

	Declare @pb int
	Declare @parentID_HienTai int
	
	Declare @hrm_EmploymentHistoryID int
	select @hrm_EmploymentHistoryID = EmploymentHistoryID, @pb = DepartmentID, @kq = NgayXuongTau, @categoryDecisionID_HienTai = CategoryDecisionID
	from view_FirstEMPLOYMENTHISTORY where EmployeeID = @employeeID	and XacNhan = 1
	
	select @parentID_HienTai = ParentID from DIC_DEPARTMENT where DepartmentID = @pb

	if (@categoryDecisionID_HienTai = 2)	-- Nếu là tuyển dụng thì lấy ngày tuyển dụng	
	begin
		RETURN @kq
	end
	else
	
		-- Add the T-SQL statements to compute the return value here	           
                         
		declare cs cursor for 
		select dbo.HRM_EMPLOYMENTHISTORY.DepartmentID, NgayXuongTau, CategoryDecisionID, dbo.DIC_DEPARTMENT.ParentID
		FROM   dbo.DIC_DEPARTMENT RIGHT OUTER JOIN
					  dbo.HRM_EMPLOYMENTHISTORY ON dbo.DIC_DEPARTMENT.DepartmentID = dbo.HRM_EMPLOYMENTHISTORY.DepartmentID
		where EmployeeID = @employeeID and XacNhan = 1 and EmploymentHistoryID <> @hrm_EmploymentHistoryID
		order by dbo.HRM_EMPLOYMENTHISTORY.DecisionDate Desc
	
		begin		
			open cs
			declare @qtct varchar(max)
			fetch next from cs into @departmentID, @ngayXT, @CategoryDecisionID, @parentID

			while @@FETCH_STATUS = 0
			begin		
				if (@departmentID is null) or (@parentID_HienTai <> @parentID)
					break		
				else
				begin
					if (@parentID_HienTai = 8) -- Khối tàu
						if (@pb <> @departmentID)
							break
					else
					begin

						set @kq = @ngayXT

						if (@CategoryDecisionID = 2)	-- Nếu là tuyển dụng thì lấy ngày tuyển dụng	
						begin				
							break		
						end
								
					end	
			
				end
				fetch next from cs into @departmentID, @ngayXT, @CategoryDecisionID, @parentID
		end
		close cs
		deallocate cs 
		
	end
	-- Return the result of the function
	--if @kq is null
	--	set @kq = @ngayXT
	--print @kq
	RETURN @kq

END

GO

create PROCEDURE  [dbo].[sp_LayDSNhanVienTheoNgay]
	-- Add the parameters for the stored procedure here	
	@ngay smallDateTime	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

SELECT EmploymentHistoryID, DecisionDate, EffectiveDate, ContentDecision, CategoryDecisionID, EmployeeID, DepartmentID, PositionID, InternshipPosition, PluralityID, IntershipPlurality, Note, PerPosition, PerPlurality, SalaryPositionID, 
                  SalaryPluralityID, DepartmentName, LoaiTauID, Gross, Power, NgayXuongTau, XacNhan, DecisionNo, DepartmentPluralityID, LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, 
                  BonusPlurality, AllowanceBonusPlurality, ThoiGianThucTap, dbo.fc_GetPositionByHRM_History(EmploymentHistoryID) as ChucVu FROM(
SELECT EmploymentHistoryID, DecisionDate, EffectiveDate, ContentDecision, CategoryDecisionID, EmployeeID, DepartmentID, PositionID, InternshipPosition, PluralityID, IntershipPlurality, Note, PerPosition, PerPlurality, SalaryPositionID, 
                  SalaryPluralityID, DepartmentName, LoaiTauID, Gross, Power, NgayXuongTau, XacNhan, DecisionNo, DepartmentPluralityID, LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, 
                  BonusPlurality, AllowanceBonusPlurality, ThoiGianThucTap,row_number() over(partition by EmployeeID order by DecisionDate desc, EmploymentHistoryID desc) as rn
FROM     dbo.HRM_EMPLOYMENTHISTORY where XacNhan = 1 and DecisionDate <= @ngay) AS T
WHERE rn = 1 and CategoryDecisionID <> 3 and EmployeeID NOT in (SELECT DISTINCT EmployeeID FROM HRM_EMPLOYMENTHISTORY WHERE CategoryDecisionID = 2 and DecisionDate>= @ngay )


END
GO