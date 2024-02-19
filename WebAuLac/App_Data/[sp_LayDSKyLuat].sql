USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDSKyLuat]    Script Date: 11/19/2018 4:17:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE  [dbo].[sp_LayDSKyLuat]
	-- Add the parameters for the stored procedure here	
	@tungay smallDateTime,
	@denngay smallDateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

	SELECT dbo.HRM_EMPLOYEE_DISCIPLINE.EmployeeDisciplineID, dbo.HRM_EMPLOYEE_DISCIPLINE.EmployeeID, dbo.HRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID, dbo.HRM_EMPLOYEE_DISCIPLINE.DisciplineNo, 
                  dbo.HRM_EMPLOYEE_DISCIPLINE.DisciplineDate, dbo.HRM_EMPLOYEE_DISCIPLINE.Reason, dbo.DIC_TYPE_OF_DISCIPLINE.TypeOfDisciplineName, dbo.viewHRM_EMPLOYMENTHISTORY.FirstName, 
                  dbo.viewHRM_EMPLOYMENTHISTORY.LastName, dbo.viewHRM_EMPLOYMENTHISTORY.BirthDay, dbo.viewHRM_EMPLOYMENTHISTORY.BirthPlace, dbo.viewHRM_EMPLOYMENTHISTORY.ChucVu,
				  [dbo].[fc_GetParentDepartmentID](dbo.HRM_EMPLOYEE_DISCIPLINE.EmployeeID)as ParentID
	FROM     dbo.viewHRM_EMPLOYMENTHISTORY RIGHT OUTER JOIN
                  dbo.HRM_EMPLOYEE_DISCIPLINE ON dbo.viewHRM_EMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE_DISCIPLINE.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_TYPE_OF_DISCIPLINE ON dbo.HRM_EMPLOYEE_DISCIPLINE.TypeOfDisciplineID = dbo.DIC_TYPE_OF_DISCIPLINE.TypeOfDisciplineID
	WHERE dbo.HRM_EMPLOYEE_DISCIPLINE.DisciplineDate>= @tungay and dbo.HRM_EMPLOYEE_DISCIPLINE.DisciplineDate <= @denngay


END
