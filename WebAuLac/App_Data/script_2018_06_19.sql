alter table HoSoTau
add NoiCap nvarchar(500) null
go
alter table HoSoCongTy
add NoiCap nvarchar(500) null
go
ALTER PROCEDURE  [dbo].[sp_LayDSChungChiTau]
AS
BEGIN
	SET NOCOUNT ON; 
 
SELECT        dbo.HoSoTau.HoSoTauID, dbo.HoSoTau.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HoSoTau.ChungChiID, dbo.ChungChiTau.TenChungChi, dbo.ChungChiTau.SoNgayCanhBao, dbo.HoSoTau.NgayCap, dbo.HoSoTau.NoiCap, 
                         dbo.HoSoTau.NgayHetHan, dbo.HoSoTau.FileDinhKem
FROM            dbo.DIC_DEPARTMENT INNER JOIN
                         dbo.HoSoTau ON dbo.DIC_DEPARTMENT.DepartmentID = dbo.HoSoTau.DepartmentID INNER JOIN
                         dbo.ChungChiTau ON dbo.HoSoTau.ChungChiID = dbo.ChungChiTau.ChungChiID
 

END

