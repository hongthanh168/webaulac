CREATE procedure [dbo].[sp_LayThongTinMatrix]
(
	@chucDanhID int,
	@employeeID int
)
as
	begin
		select [dbo].[fc_T_CrewMatrix_WithOperator](@employeeID) as WithOperator,
        [dbo].[fc_T_CrewMatrix_InRank] (@chucDanhID, @employeeID) as InRank,
		[dbo].[fc_T_CrewMatrix_ThisTypeOfTanker] (@employeeID) as ThisTypeOfTanker,
		[dbo].[fc_T_CrewMatrix_AllTypeOfTanker] (@employeeID) as AllTypeOfTanker,
        [dbo].[fc_T_CrewMatrix_WatchKeeping](@employeeID) as WatchKeeping
	end
GO


