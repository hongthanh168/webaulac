USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDSBoNhiem]    Script Date: 11/19/2018 4:17:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE  [dbo].[sp_LayDSBoNhiem]
	-- Add the parameters for the stored procedure here	
	@tungay smallDateTime,
	@denngay smallDateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

	select EmployeeID, EmploymentHistoryID, FirstName, LastName, ParentID, DepartmentID, PositionID,
	DecisionDate, CategoryDecisionName, DepartmentName, ChucVu 
	from view_quatrinhcongtacFull 
	where DecisionDate >= @tungay and DecisionDate <= @denngay and CategoryDecisionID = 1
	order by PositionID asc, DecisionDate desc

END
