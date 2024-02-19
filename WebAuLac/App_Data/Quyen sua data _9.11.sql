/*
   Tuesday, October 30, 20189:43:02 AM
   User: 
   Server: WIN-0UJ5BAEK0M9\SQLEXPRESS
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
ALTER TABLE dbo.HRM_EMPLOYEE SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE ADD CONSTRAINT
	FK_HRM_EMPLOYEE_DEGREE_HRM_EMPLOYEE FOREIGN KEY
	(
	EmployeeID
	) REFERENCES dbo.HRM_EMPLOYEE
	(
	EmployeeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.HRM_EMPLOYEE_DEGREE SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--////////////////////////////////////////////////////////
--Sửa thêm biến phân ra thành thuyền viên và nhân viên văn phòng
USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDSNghiViec]    Script Date: 11/9/2018 10:13:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE  [dbo].[sp_LayDSNghiViec]
	-- Add the parameters for the stored procedure here	
	@tungay smallDateTime,
	@denngay smallDateTime, 
	@loainv int --1 là thuyền viên, 2 là nhân viên vp
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	if (@loainv = 1)
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
				  	Where  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17
		 
	else
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
	Where  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) is null  or  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 1
			  

END
