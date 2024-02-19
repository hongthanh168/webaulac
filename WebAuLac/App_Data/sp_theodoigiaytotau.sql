USE [AuLac]
GO
/****** Object:  StoredProcedure [dbo].[sp_BangTheoDoiGiayToTau]    Script Date: 10/26/2018 9:48:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE  [dbo].[sp_BangTheoDoiGiayToTau]
	-- Add the parameters for the stored procedure here	
	@ngayBaoCao SmallDatetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @thang int, @nam int
	SET @thang = MONTH(@ngayBaoCao)
	SET @nam = YEAR(@ngayBaoCao)
	

	SELECT * , dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang, @nam, ChungChiID) as Thang1,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 1, @nam, ChungChiID) as Thang2,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 2, @nam, ChungChiID) as Thang3,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 3, @nam, ChungChiID) as Thang4,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 4, @nam, ChungChiID) as Thang5,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 5, @nam, ChungChiID) as Thang6,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 6, @nam, ChungChiID) as Thang7,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 7, @nam, ChungChiID) as Thang8,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 8, @nam, ChungChiID) as Thang9,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 9, @nam, ChungChiID) as Thang10,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 10, @nam, ChungChiID) as Thang11,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 11, @nam, ChungChiID) as Thang12,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 12, @nam, ChungChiID) as Thang13,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 13, @nam, ChungChiID) as Thang14,
	dbo.fc_LayDSTauHetHanTheoChungChi_ThoiGian( @thang + 14, @nam, ChungChiID) as Thang15, 
	@ngayBaoCao as NgayBaoCao
	FROM ChungChiTau order by STT

END