USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDSTuyenDung]    Script Date: 11/14/2018 9:26:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE  [dbo].[sp_LayDSTuyenDung]
	-- Add the parameters for the stored procedure here	
	@tungay smallDateTime,
	@denngay smallDateTime,
	@loainv int --1 là thuyền viên, 2 là nhân viên vp
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 
	if(@loainv = 1)
	SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.QuanHeID, EMHIS.EmploymentHistoryID, EMHIS.DecisionNo, 
                  EMHIS.DecisionDate, EMHIS.Note, dbo.DIC_POSITION.PositionID, dbo.DIC_POSITION.PositionName, dbo.DIC_POSITION.GroupPositionID, dbo.DIC_EDUCATION.EducationID, 
                  dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.Sex, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.EmployeeCode, 
                  EMCONTRACT.ContractHistoryID, EMCONTRACT.ContractTypeID, EMCONTRACT.ContractNo, EMCONTRACT.ContractDate, 
                  dbo.DIC_CONTRACTTYPE.ContractTypeName, dbo.DIC_CONTRACTTYPE.GroupContractTypeID, dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeName,				  
				  dbo.DIC_DEPARTMENT.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, case when (dbo.HRM_EMPLOYEE.BirthDay is null) then null else DATEDIFF(yy, dbo.HRM_EMPLOYEE.BirthDay, GETDATE()) end as Tuoi
	FROM     (SELECT * FROM dbo.HRM_EMPLOYMENTHISTORY WHERE XacNhan = 1 and CategoryDecisionID = 2 and DecisionDate >= @tungay and DecisionDate<= @denngay) as EMHIS INNER JOIN
                  dbo.HRM_EMPLOYEE ON EMHIS.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID INNER JOIN
                  dbo.DIC_POSITION ON EMHIS.PositionID = dbo.DIC_POSITION.PositionID INNER JOIN
                  dbo.DIC_DEPARTMENT ON EMHIS.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                  dbo.DIC_CONTRACTTYPE INNER JOIN
                  dbo.DIC_GROUPCONTRACTTYPE ON dbo.DIC_CONTRACTTYPE.GroupContractTypeID = dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeID INNER JOIN
                  (SELECT MAX(ContractDate) as ContractDate, max(ContractHistoryID) as ContractHistoryID, 
				  MAX (ContractTypeID) AS ContractTypeID, MAX (ContractNo) AS ContractNo, MAX(EmployeeID) AS EmployeeID			  
				  FROM dbo.HRM_CONTRACTHISTORY GROUP BY EmployeeID) as EMCONTRACT ON dbo.DIC_CONTRACTTYPE.ContractTypeID = EMCONTRACT.ContractTypeID ON dbo.HRM_EMPLOYEE.EmployeeID = EMCONTRACT.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID
	Where  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 8 or dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID)= 17
	else
	SELECT dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.QuanHeID, EMHIS.EmploymentHistoryID, EMHIS.DecisionNo, 
                  EMHIS.DecisionDate, EMHIS.Note, dbo.DIC_POSITION.PositionID, dbo.DIC_POSITION.PositionName, dbo.DIC_POSITION.GroupPositionID, dbo.DIC_EDUCATION.EducationID, 
                  dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.Sex, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.EmployeeCode, 
                  EMCONTRACT.ContractHistoryID, EMCONTRACT.ContractTypeID, EMCONTRACT.ContractNo, EMCONTRACT.ContractDate, 
                  dbo.DIC_CONTRACTTYPE.ContractTypeName, dbo.DIC_CONTRACTTYPE.GroupContractTypeID, dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeName,				  
				  dbo.DIC_DEPARTMENT.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, case when (dbo.HRM_EMPLOYEE.BirthDay is null) then null else DATEDIFF(yy, dbo.HRM_EMPLOYEE.BirthDay, GETDATE()) end as Tuoi
	FROM     (SELECT * FROM dbo.HRM_EMPLOYMENTHISTORY WHERE XacNhan = 1 and CategoryDecisionID = 2 and DecisionDate >= @tungay and DecisionDate<= @denngay) as EMHIS INNER JOIN
                  dbo.HRM_EMPLOYEE ON EMHIS.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID INNER JOIN
                  dbo.DIC_POSITION ON EMHIS.PositionID = dbo.DIC_POSITION.PositionID INNER JOIN
                  dbo.DIC_DEPARTMENT ON EMHIS.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                  dbo.DIC_CONTRACTTYPE INNER JOIN
                  dbo.DIC_GROUPCONTRACTTYPE ON dbo.DIC_CONTRACTTYPE.GroupContractTypeID = dbo.DIC_GROUPCONTRACTTYPE.GroupContractTypeID INNER JOIN
                  (SELECT MAX(ContractDate) as ContractDate, max(ContractHistoryID) as ContractHistoryID, 
				  MAX (ContractTypeID) AS ContractTypeID, MAX (ContractNo) AS ContractNo, MAX(EmployeeID) AS EmployeeID			  
				  FROM dbo.HRM_CONTRACTHISTORY GROUP BY EmployeeID) as EMCONTRACT ON dbo.DIC_CONTRACTTYPE.ContractTypeID = EMCONTRACT.ContractTypeID ON dbo.HRM_EMPLOYEE.EmployeeID = EMCONTRACT.EmployeeID LEFT OUTER JOIN
                  dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID
Where  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) is null  or  dbo.fc_GetParentDepartment(dbo.HRM_EMPLOYEE.EmployeeID) = 1
END

