USE [AuLac]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_DegreeLimitList_NhanVien]    Script Date: 11/15/2018 3:29:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_DegreeLimitList_NhanVien]
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
WHERE   (GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate) and (dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) is null or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 1)
           and dbo.HRM_EMPLOYEE.StatusID = 1 

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
          ( dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate > GETDATE() and dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore )
		   and(dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) is null or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 1)
           and dbo.HRM_EMPLOYEE.StatusID = 1)
		   
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
--UNION 
--SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName,
--  dbo.HRM_EMPLOYEE.BirthDay,
--  dbo.HRM_EMPLOYEE.BirthPlace, 
--   dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
-- (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,

-- dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo, 
-- dbo.DIC_DEGREE.DegreeName,
-- CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
-- CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(3 AS int)
-- FROM            dbo.DIC_DEGREE INNER JOIN
--                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
--						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
--WHERE   GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate
--or (GETDATE() < dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate and   dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore) )
--  and dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17
--           and dbo.HRM_EMPLOYEE.StatusID = 1
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
WHERE  dbo.HRM_EMPLOYEE_DEGREE.IsBC =1	and (dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) is null or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 1)
           and dbo.HRM_EMPLOYEE.StatusID = 1
		  )
		  ---------------THUYỀN VIÊN
USE [AuLac]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_DegreeLimitList]    Script Date: 11/16/2018 11:04:44 AM ******/
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
WHERE   (GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate) and (dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17)
           and dbo.HRM_EMPLOYEE.StatusID = 1 

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
          ( dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate > GETDATE() and dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore )
		   and(dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17)
           and dbo.HRM_EMPLOYEE.StatusID = 1)
		   
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
--UNION 
--SELECT dbo.HRM_EMPLOYEE.EmployeeID,dbo.HRM_EMPLOYEE.FirstName + ' ' + dbo.HRM_EMPLOYEE.LastName as EmployeeName,
--  dbo.HRM_EMPLOYEE.BirthDay,
--  dbo.HRM_EMPLOYEE.BirthPlace, 
--   dbo.fc_GetPosition(dbo.HRM_EMPLOYEE.EmployeeID) as PositionName,
-- (select DepartmentName from dbo.DIC_DEPARTMENT d where d.DepartmentID = dbo.fc_GetDepartment(dbo.HRM_EMPLOYEE.EmployeeID)) as DepartmentName,

-- dbo.HRM_EMPLOYEE_DEGREE.DegreeID, dbo.HRM_EMPLOYEE_DEGREE.DegreeNo, 
-- dbo.DIC_DEGREE.DegreeName,
-- CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.DegreeDate, 103) as DegreeDate,
-- CONVERT(varchar, dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate, 103) as ExpirationDate, TH= CAST(3 AS int)
-- FROM            dbo.DIC_DEGREE INNER JOIN
--                         dbo.HRM_EMPLOYEE_DEGREE ON dbo.DIC_DEGREE.DegreeID = dbo.HRM_EMPLOYEE_DEGREE.DegreeID INNER JOIN
--						 dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYEE_DEGREE.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID
--WHERE   GETDATE() >= dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate
--or (GETDATE() < dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate and   dbo.HRM_EMPLOYEE_DEGREE.ExpirationDate <= (GETDATE() + dbo.DIC_DEGREE.WarningBefore) )
--  and dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17
--           and dbo.HRM_EMPLOYEE.StatusID = 1
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
WHERE  dbo.HRM_EMPLOYEE_DEGREE.IsBC =1	and (dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17)
           and dbo.HRM_EMPLOYEE.StatusID = 1
		  )


		  --------------------------------------------

 