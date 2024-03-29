/*
   Monday, August 27, 201811:26:29 AM
   User: 
   Server: QPC\SQLEXPRESS
   Database: AuLac
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE
	DROP CONSTRAINT FK_HRM_EMPLOYEE_DEGREE_HRM_EMPLOYEE_NEW
GO
ALTER TABLE dbo.HRM_EMPLOYEE SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.DIC_POSITION SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE ADD CONSTRAINT
	FK_HRM_EMPLOYEE_DEGREE_HRM_EMPLOYEE_NEW FOREIGN KEY
	(
	PositionID
	) REFERENCES dbo.DIC_POSITION
	(
	PositionID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE ADD CONSTRAINT
	FK_HRM_EMPLOYEE_DEGREE_HRM_EMPLOYEE_DEGREE FOREIGN KEY
	(
	EmployeeDegreeID
	) REFERENCES dbo.HRM_EMPLOYEE_DEGREE
	(
	EmployeeDegreeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

USE [AuLac]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_DegreeLimitList]    Script Date: 8/30/2018 11:12:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
ALTER FUNCTION [dbo].[fn_DegreeLimitList]
(	
 
)
RETURNS TABLE 
AS
RETURN 
(
 
 SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName,
  dbo.HRM_EMPLOYEE.BirthDay,
  dbo.HRM_EMPLOYEE.BirthPlace, 
   dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
 (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,

 dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo, 
 dbo.DIC_DEGREE.DegreeName,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(0 AS int)
 FROM            dbo.DIC_DEGREE INNER JOIN
                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
WHERE   GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate
           
UNION

 
 SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName, 
 dbo.HRM_EMPLOYEE.BirthDay, 
 dbo.HRM_EMPLOYEE.BirthPlace, 
  dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
 (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,
 dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo,
 dbo.DIC_DEGREE.DegreeName, 
CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(1 AS int)
 FROM            dbo.DIC_DEGREE INNER JOIN
                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
WHERE  
           GETDATE() < dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate AND   dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore )

--UNION 
-- SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName, 
-- dbo.HRM_EMPLOYEE.BirthDay, 
-- dbo.HRM_EMPLOYEE.BirthPlace, 
--  dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
-- (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,
-- dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo,
-- dbo.DIC_DEGREE.DegreeName, 
--CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
-- CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(2 AS int)
-- FROM            dbo.DIC_DEGREE INNER JOIN
--                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
--						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
UNION 
SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName,
  dbo.HRM_EMPLOYEE.BirthDay,
  dbo.HRM_EMPLOYEE.BirthPlace, 
   dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
 (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,

 dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo, 
 dbo.DIC_DEGREE.DegreeName,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(3 AS int)
 FROM            dbo.DIC_DEGREE INNER JOIN
                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
WHERE   GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate
or (GETDATE() < dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate AND   dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore) )
 
 UNION
 
 SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName,
  dbo.HRM_EMPLOYEE.BirthDay,
  dbo.HRM_EMPLOYEE.BirthPlace, 
   dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
 (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,

 dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo, 
 dbo.DIC_DEGREE.DegreeName,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
 CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(4 AS int)
 FROM            dbo.DIC_DEGREE INNER JOIN
                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
WHERE  dbo.HRM_EMPLOYEE_DEGREE.IsBC =1	
		  )


		  --------------------------------------------



