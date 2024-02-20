USE [AuLac]
GO

/****** Object:  View [dbo].[view_quatrinhcongtacFull]    Script Date: 11/19/2018 4:18:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
                  dbo.HRM_EMPLOYEE.Note AS Expr2, dbo.DIC_DEPARTMENT.ParentID, dbo.DIC_LOAITAU.TenLoaiTau, dbo.HRM_EMPLOYMENTHISTORY.LyDoNghiViec_ID, dbo.DIC_CATEGORYDECISION.CategoryDecisionName
FROM     dbo.HRM_EMPLOYMENTHISTORY INNER JOIN
                  dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_CATEGORYDECISION ON dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID = dbo.DIC_CATEGORYDECISION.CategoryDecisionID LEFT OUTER JOIN
                  dbo.DIC_LOAITAU ON dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID = dbo.DIC_LOAITAU.LoaiTauID LEFT OUTER JOIN
                  dbo.DIC_DEPARTMENT ON dbo.HRM_EMPLOYMENTHISTORY.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP ON dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                  dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                  dbo.DIC_POSITION AS DIC_POSITION_1 ON dbo.HRM_EMPLOYMENTHISTORY.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                  dbo.DIC_POSITION ON dbo.HRM_EMPLOYMENTHISTORY.PositionID = dbo.DIC_POSITION.PositionID


GO


