USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_T_LayDanhSachHocTheoThoiGian]    Script Date: 11/14/2018 5:01:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_T_LayDanhSachHocTheoThoiGian]
@tuNgay smalldatetime,
@denNgay smalldatetime
as 
	begin
		select h.EmployeeID, h.LastName, h.FirstName, h.EmployeeCode, h.BirthDay, dbo.fc_GetPosition(h.EmployeeID)as ChucDanh,
		ct.id_ctdaotao, ct.ketqua, k.tenkhoadaotao, k.ngaybatdau, k.NgayKetThuc, k.diadiem, k.hocphi, k.TheoYeuCau, cs.tencoso, v.ParentID
		from HRM_EMPLOYEE h, tbl_ctdaotao ct, tbl_khoadaotao k, tbl_cosodaotao cs, viewHRM_EMPLOYMENTHISTORY v
		where h.EmployeeID = ct.EmployeeID
		and h.EmployeeID = v.EmployeeID
		and ct.id_khoadaotao = k.id_khoadaotao
		and k.id_cosodaotao = cs.id_cosodaotao
		and k.ngaybatdau between @tuNgay and @denNgay
	end
go
-----------------------------------------
----------CAC PHAN CUA ANH SY-------------
------------------------------------------------------
CREATE VIEW [dbo].[view_EMPLOYMENTHISTORY_DauNam]
AS
SELECT EmploymentHistoryID, DecisionDate, EffectiveDate, ContentDecision, CategoryDecisionID, EmployeeID, DepartmentID, PositionID, InternshipPosition, PluralityID, IntershipPlurality, Note, PerPosition, PerPlurality, SalaryPositionID, 
                  SalaryPluralityID, DepartmentName, LoaiTauID, Gross, Power, NgayXuongTau, XacNhan, DecisionNo, DepartmentPluralityID, LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, 
                  BonusPlurality, AllowanceBonusPlurality, ThoiGianThucTap FROM(
SELECT EmploymentHistoryID, DecisionDate, EffectiveDate, ContentDecision, CategoryDecisionID, EmployeeID, DepartmentID, PositionID, InternshipPosition, PluralityID, IntershipPlurality, Note, PerPosition, PerPlurality, SalaryPositionID, 
                  SalaryPluralityID, DepartmentName, LoaiTauID, Gross, Power, NgayXuongTau, XacNhan, DecisionNo, DepartmentPluralityID, LyDoNghiViec_ID, Salary, AllowanceSalary, Bonus, AllowanceBonus, SalaryPlurality, AllowanceSalaryPlurality, 
                  BonusPlurality, AllowanceBonusPlurality, ThoiGianThucTap,row_number() over(partition by EmployeeID order by DecisionDate desc, EmploymentHistoryID desc) as rn
FROM     dbo.HRM_EMPLOYMENTHISTORY where XacNhan = 1 and year(DecisionDate) < Year(getdate())) AS T
WHERE rn = 1 and EmployeeID NOT in (SELECT DISTINCT EmployeeID FROM HRM_EMPLOYMENTHISTORY WHERE CategoryDecisionID = 2 and YEAR(DecisionDate) = Year(getdate()))

GO

GO

/*--------------Thanh sửa lại để tính ghi chú*/
ALTER VIEW [dbo].[viewHRM_EMPLOYMENTHISTORY]
AS
SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, TT.EmploymentHistoryID, 
                  TT.DecisionNo, TT.DecisionDate, TT.ContentDecision, TT.CategoryDecisionID, TT.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, TT.PositionID, dbo.DIC_POSITION.PositionName, TT.InternshipPosition, TT.PluralityID, 
                  DIC_POSITION_1.PositionName AS PluralityName, TT.IntershipPlurality, dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, 
                  dbo.fc_GetPosition(TT.EmployeeID) AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, 
                  DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, TT.PerPosition, TT.PerPlurality, TT.SalaryPositionID, TT.SalaryPluralityID, TT.EffectiveDate, dbo.HRM_EMPLOYEE.QuanHeID, dbo.HRM_EMPLOYEE.MainAddress, 
                  dbo.HRM_EMPLOYEE.CellPhone, dbo.HRM_EMPLOYEE.IDCard, dbo.HRM_EMPLOYEE.IDCardDate, dbo.HRM_EMPLOYEE.IDCardPlace, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.Origin, dbo.DIC_SCHOOL.SchoolName, 
                  dbo.DIC_SCHOOL.Description AS SchoolVT, dbo.fc_NgayXuongTauvaVeDuTru(dbo.HRM_EMPLOYEE.EmployeeID) AS NgayXuongTau, dbo.HRM_EMPLOYEE.StatusID, TT.LoaiTauID, dbo.HRM_EMPLOYEE.GhiChuSSDD, TT.Note, 
                  dbo.DIC_DEPARTMENT.ParentID, dbo.HRM_EMPLOYEE.SSDD
FROM     dbo.view_FirstEMPLOYMENTHISTORY AS TT INNER JOIN
                  dbo.HRM_EMPLOYEE ON TT.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID INNER JOIN
                  dbo.DIC_DEPARTMENT ON TT.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                  dbo.DIC_SCHOOL ON dbo.HRM_EMPLOYEE.SchoolID = dbo.DIC_SCHOOL.SchoolID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP ON TT.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON TT.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                  dbo.DIC_POSITION AS DIC_POSITION_1 ON TT.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                  dbo.DIC_POSITION ON TT.PositionID = dbo.DIC_POSITION.PositionID

GO

ALTER VIEW [dbo].[view_quatrinhcongtacFull]
AS
SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, 
                  dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID, dbo.HRM_EMPLOYMENTHISTORY.DecisionNo, dbo.HRM_EMPLOYMENTHISTORY.DecisionDate, dbo.HRM_EMPLOYMENTHISTORY.ContentDecision, 
                  dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentID, (CASE WHEN dbo.HRM_EMPLOYMENTHISTORY.DepartmentID IS NOT NULL 
                  THEN dbo.DIC_DEPARTMENT.DepartmentName ELSE dbo.HRM_EMPLOYMENTHISTORY.DepartmentName END) AS DepartmentName, dbo.HRM_EMPLOYMENTHISTORY.PositionID, dbo.DIC_POSITION.PositionName, 
                  dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition, dbo.HRM_EMPLOYMENTHISTORY.PluralityID, DIC_POSITION_1.PositionName AS PluralityName, dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality, 
                  dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, dbo.fc_GetPositionByHRM_History(dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID) 
                  AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, 
                  DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, dbo.HRM_EMPLOYMENTHISTORY.EffectiveDate, dbo.HRM_EMPLOYMENTHISTORY.Note, dbo.HRM_EMPLOYMENTHISTORY.PerPosition, 
                  dbo.HRM_EMPLOYMENTHISTORY.PerPlurality, dbo.HRM_EMPLOYMENTHISTORY.SalaryPositionID, dbo.HRM_EMPLOYMENTHISTORY.SalaryPluralityID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentName AS Expr1, 
                  dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID, dbo.HRM_EMPLOYMENTHISTORY.Gross, dbo.HRM_EMPLOYMENTHISTORY.Power, dbo.HRM_EMPLOYMENTHISTORY.NgayXuongTau, dbo.HRM_EMPLOYMENTHISTORY.XacNhan, 
                  dbo.HRM_EMPLOYEE.CardNo, dbo.HRM_EMPLOYEE.MainAddress, dbo.HRM_EMPLOYEE.CellPhone, dbo.HRM_EMPLOYEE.HomePhone, dbo.HRM_EMPLOYEE.IDCard, dbo.HRM_EMPLOYEE.IDCardDate, 
                  dbo.HRM_EMPLOYEE.IDCardPlace, dbo.HRM_EMPLOYEE.TaxNo, dbo.HRM_EMPLOYEE.BankCode, dbo.HRM_EMPLOYEE.BankName, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.QuanHeID, 
                  dbo.HRM_EMPLOYEE.Note AS Expr2, dbo.DIC_DEPARTMENT.ParentID, dbo.DIC_LOAITAU.TenLoaiTau, dbo.HRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID
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
create FUNCTION [dbo].[fc_LayDSThuyenVienChuaLapQuyetDinh] 
(
	-- Add the parameters for the function here
	@planID int
)
RETURNS nvarchar(max)
AS
BEGIN
	
	Declare @kq nvarchar(max)
	set @kq =''
	Declare @lastName nvarchar(50)

	declare cs cursor for 
	select LastName from HRM_EMPLOYEE
	where EmployeeID in (SELECT CrewOffID FROM HRM_DETAILPLAN WHERE PlanID = @planID and (CrewOffHistoryID is null or CrewOffID = 0 ))

	open cs	
	fetch next from cs into @lastName

	while @@FETCH_STATUS = 0
	begin		
		set @kq =  @kq + ', ' + @lastName	
				
		fetch next from cs into @lastName
	end
	close cs
	deallocate cs 

	--SELECT CrewOnID, CrewOnHistoryID FROM HRM_DETAILPLAN WHERE PlanID = 4015 and CrewOnHistoryID is not null 
	
	RETURN @kq

END

GO
Create VIEW [dbo].[view_HRM_PLAN]
AS
SELECT PlanID, PlanName, PlanDate, Note, IsLock, dbo.fc_LayDSThuyenVienChuaLapQuyetDinh(PlanID) AS ChuaLapQuyetDinh
FROM     dbo.HRM_PLAN

