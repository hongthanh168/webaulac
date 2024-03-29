USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_T_LayDanhSachNhanVien]    Script Date: 11/26/2018 7:58:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------thêm việc lấy ParentDepartment
ALTER procedure [dbo].[sp_T_LayDanhSachNhanVien]
as
begin
	select e.EmployeeID, e.EmployeeCode, e.FirstName, e.LastName,e.BirthDay, e.BirthPlace, 
                         v.DepartmentID, v.DepartmentName, v.PositionID, v.EducationName,
                         v.PositionName, v.ChucVu,
						 dbo.fc_GetParentDepartment(v.EmployeeID) as ParentDepartment, 
						 e.ContactAddress, e.StatusID, k.*, e.Note, e.QuanHeID, e.LoaiQuanHe, q.HoTen as TenMoiQuanHe,
						 e.ContactAddress_Tinh as eContacAddress_Tinh, e.ContactAddress_Huyen as eContactAddress_Huyen,
						 e.Origin as eOrigin, e.Origin_Tinh as eOrigin_Tinh, e.Origin_Huyen as eOrigin_Huyen,
						 e.MainAddress as eMainAddress, e.MainAddress_Tinh as eMainAddress_Tinh, e.MainAddress_Huyen as eMainAddress_Huyen
	from HRM_EMPLOYEE e 
		Left outer join viewHRM_EMPLOYMENTHISTORY v on v.EmployeeID = e.EmployeeID	 
		Left outer join DIC_KHUVUC k on k.KhuVucID = e.KhuVucID	 
		Left outer join tbl_QuanHe q on q.QuanHeID = e.QuanHeID	
end
