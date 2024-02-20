alter table HoSoCongTy
add STT int null
go
alter table ChungChiTau
add STT int null
go
alter table DIC_DEGREE
add STT int null
go
update HoSoCongTy
set STT = HoSoCongTyID
go
update ChungChiTau
set STT = ChungChiID
go 
-------------sửa lại store lấy chứng chỉ tàu
ALTER PROCEDURE  [dbo].[sp_LayDSChungChiTau]
AS
BEGIN
	SET NOCOUNT ON; 
 
SELECT        dbo.HoSoTau.HoSoTauID, dbo.HoSoTau.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HoSoTau.ChungChiID, dbo.ChungChiTau.TenChungChi, dbo.ChungChiTau.STT, dbo.HoSoTau.SoHoSo, dbo.ChungChiTau.SoNgayCanhBao, dbo.HoSoTau.NgayCap, dbo.HoSoTau.NoiCap, 
                         dbo.HoSoTau.NgayHetHan, dbo.HoSoTau.FileDinhKem
FROM            dbo.DIC_DEPARTMENT INNER JOIN
                         dbo.HoSoTau ON dbo.DIC_DEPARTMENT.DepartmentID = dbo.HoSoTau.DepartmentID INNER JOIN
                         dbo.ChungChiTau ON dbo.HoSoTau.ChungChiID = dbo.ChungChiTau.ChungChiID
 

END
go
---store dùng để update STT của hồ sơ công ty
create procedure sp_T_UpdateSTTLenXuong
(
	@id int, 
	@isLen int

)
as
	begin
		--lấy ra STT hiện tại của nó
		declare @sttHienTai int
		select @sttHienTai = STT
		from HoSoCongTy
		where HoSoCongTyID = @id

		--tìm ra số STT lớn nhất mà gần nó nhất
		declare @sttGanNhat int
		set @sttGanNhat = @sttHienTai
		if @isLen =1
			begin
				if (exists (select * from HoSoCongTy where STT<@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from HoSoCongTy
											where STT<@sttHienTai
											order by STT desc
										  )
					end
				end
		else
			begin
				if (exists (select * from HoSoCongTy where STT>@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from HoSoCongTy
											where STT>@sttHienTai
											order by STT asc
										  )
					end
			end		
		if (@sttGanNhat<> @sttHienTai)
			begin				
				--cập nhật lại 2 số STT
				update HoSoCongTy
				set STT = @sttHienTai
				where STT = @sttGanNhat
			
				update HoSoCongTy
				set STT = @sttGanNhat
				where HoSoCongTyID = @id
			end
		return
	end
go
---store dùng để update STT của chứng chỉ tàu
create procedure sp_T_UpdateSTTLenXuong_ChungChiTau
(
	@id int, 
	@isLen int

)
as
	begin
		--lấy ra STT hiện tại của nó
		declare @sttHienTai int
		select @sttHienTai = STT
		from ChungChiTau
		where ChungChiID = @id

		--tìm ra số STT lớn nhất mà gần nó nhất
		declare @sttGanNhat int
		set @sttGanNhat = @sttHienTai
		if @isLen =1
			begin
				if (exists (select * from ChungChiTau where STT<@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from ChungChiTau
											where STT<@sttHienTai
											order by STT desc
										  )
					end
				end
		else
			begin
				if (exists (select * from ChungChiTau where STT>@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from ChungChiTau
											where STT>@sttHienTai
											order by STT asc
										  )
					end
			end		
		if (@sttGanNhat<> @sttHienTai)
			begin				
				--cập nhật lại 2 số STT
				update ChungChiTau
				set STT = @sttHienTai
				where STT = @sttGanNhat
			
				update ChungChiTau
				set STT = @sttGanNhat
				where ChungChiID = @id
			end
		return
	end
GO
ALTER VIEW [dbo].[view_quatrinhcongtacFull]
AS
SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, 
                  dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID, dbo.HRM_EMPLOYMENTHISTORY.DecisionNo, dbo.HRM_EMPLOYMENTHISTORY.DecisionDate, dbo.HRM_EMPLOYMENTHISTORY.ContentDecision, 
                  dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HRM_EMPLOYMENTHISTORY.PositionID, dbo.DIC_POSITION.PositionName, 
                  dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition, dbo.HRM_EMPLOYMENTHISTORY.PluralityID, DIC_POSITION_1.PositionName AS PluralityName, dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality, 
                  dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, dbo.fc_GetPositionByHRM_History(dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID) 
                  AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, 
                  DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, dbo.HRM_EMPLOYMENTHISTORY.EffectiveDate, dbo.HRM_EMPLOYMENTHISTORY.Note, dbo.HRM_EMPLOYMENTHISTORY.PerPosition, 
                  dbo.HRM_EMPLOYMENTHISTORY.PerPlurality, dbo.HRM_EMPLOYMENTHISTORY.SalaryPositionID, dbo.HRM_EMPLOYMENTHISTORY.SalaryPluralityID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentName AS Expr1, 
                  dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID, dbo.HRM_EMPLOYMENTHISTORY.Gross, dbo.HRM_EMPLOYMENTHISTORY.Power, dbo.HRM_EMPLOYMENTHISTORY.NgayXuongTau, dbo.HRM_EMPLOYMENTHISTORY.XacNhan, 
                  dbo.HRM_EMPLOYEE.CardNo, dbo.HRM_EMPLOYEE.MainAddress, dbo.HRM_EMPLOYEE.CellPhone, dbo.HRM_EMPLOYEE.HomePhone, dbo.HRM_EMPLOYEE.IDCard, dbo.HRM_EMPLOYEE.IDCardDate, 
                  dbo.HRM_EMPLOYEE.IDCardPlace, dbo.HRM_EMPLOYEE.TaxNo, dbo.HRM_EMPLOYEE.BankCode, dbo.HRM_EMPLOYEE.BankName, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.QuanHeID, 
                  dbo.HRM_EMPLOYEE.Note AS Expr2, dbo.DIC_DEPARTMENT.ParentID, dbo.DIC_LOAITAU.TenLoaiTau
FROM     dbo.HRM_EMPLOYMENTHISTORY INNER JOIN
                  dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_LOAITAU ON dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID = dbo.DIC_LOAITAU.LoaiTauID LEFT OUTER JOIN
                  dbo.DIC_DEPARTMENT ON dbo.HRM_EMPLOYMENTHISTORY.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP ON dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                  dbo.DIC_POSITION AS DIC_POSITION_1 ON dbo.HRM_EMPLOYMENTHISTORY.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                  dbo.DIC_POSITION ON dbo.HRM_EMPLOYMENTHISTORY.PositionID = dbo.DIC_POSITION.PositionID

GO


GO
create PROCEDURE [dbo].[sp_GetYMD](@date smalldatetime,@date2 smalldatetime)
 as

    begin

    declare @date3 smalldatetime

    Declare @month int,@year int,@day int

     if @date>@date2
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

    select (case when @year=0 then '' else convert(varchar(50),@year ) + 'y' end)
    + (case when @month=0 then '' else ' ' + convert(varchar(50),@month ) + 'm' end)
    + (case when @day=0 then '' else ' ' + convert(varchar(50),@day ) + 'd' end)

    end

GO

ALTER PROCEDURE  [dbo].[sp_LayDSNghiViec]
	-- Add the parameters for the stored procedure here	
	@tungay smallDateTime,
	@denngay smallDateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

	SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.QuanHeID, EMHIS.EmploymentHistoryID, EMHIS.DecisionNo, 
                  EMHIS.DecisionDate, EMHIS.Note, dbo.DIC_POSITION.PositionID, dbo.DIC_POSITION.PositionName, dbo.DIC_POSITION.GroupPositionID, dbo.DIC_EDUCATION.EducationID, 
                  dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.Sex, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.EmployeeCode, 
                  EMCONTRACT.ContractHistoryID, EMCONTRACT.ContractTypeID, EMCONTRACT.ContractNo, EMCONTRACT.ContractDate, 
                  dbo.DIC_CONTRACTTYPE.ContractTypeName, dbo.DIC_CONTRACTTYPE.GroupContractTypeID, dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeName,
				  dbo.tbl_LyDoNghiViec.LyDoNghiViec_ID, dbo.tbl_LyDoNghiViec.LyDoNghiViec_Name,
				  dbo.fc_QuaTrinhDiTau(dbo.HRM_EMPLOYEE.EmployeeID) as QTDT, dbo.fc_GetYMD(EMHIS.DecisionDate, GETDATE()) as TGNV,
				  dbo.DIC_DEPARTMENT.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName
	FROM     dbo.tbl_LyDoNghiViec RIGHT OUTER JOIN
	(SELECT * FROM dbo.HRM_EMPLOYMENTHISTORY WHERE XacNhan = 1 and CategoryDecisionID = 3 and DecisionDate >= @tungay and DecisionDate<= @denngay) as EMHIS INNER JOIN
                  dbo.HRM_EMPLOYEE ON EMHIS.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID INNER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                  dbo.DIC_POSITION ON EMHIS.PositionID = dbo.DIC_POSITION.PositionID LEFT OUTER JOIN
                  dbo.DIC_DEPARTMENT ON EMHIS.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID ON dbo.tbl_LyDoNghiViec.LyDoNghiViec_ID = EMHIS.LyDoNghiViec_ID LEFT OUTER JOIN
                  dbo.DIC_CONTRACTTYPE INNER JOIN
                  dbo.HRM_CONTRACTHISTORY as EMCONTRACT ON dbo.DIC_CONTRACTTYPE.ContractTypeID = EMCONTRACT.ContractTypeID INNER JOIN
                  dbo.DIC_GROUPCONTRACTTYPE ON dbo.DIC_CONTRACTTYPE.GroupContractTypeID = dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeID ON dbo.HRM_EMPLOYEE.EmployeeID = EMCONTRACT.EmployeeID
				  

END
GO