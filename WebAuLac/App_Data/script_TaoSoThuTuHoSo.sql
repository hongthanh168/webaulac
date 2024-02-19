alter table HoSoCongTy
add STT int null
go
alter table ChungChiTau
add STT int null
go
update HoSoCongTy
set STT = HoSoCongTyID
go
update ChungChiTau
set STT = ChungChiID
go 
-------------sửa lại store lấy chứng chỉ tàu
ALTER PROCEDURE  [dbo].[sp_LayDSChungChiTau]
AS
BEGIN
	SET NOCOUNT ON; 
 
SELECT        dbo.HoSoTau.HoSoTauID, dbo.HoSoTau.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HoSoTau.ChungChiID, dbo.ChungChiTau.TenChungChi, dbo.ChungChiTau.STT, dbo.HoSoTau.SoHoSo, dbo.ChungChiTau.SoNgayCanhBao, dbo.HoSoTau.NgayCap, dbo.HoSoTau.NoiCap, 
                         dbo.HoSoTau.NgayHetHan, dbo.HoSoTau.FileDinhKem
FROM            dbo.DIC_DEPARTMENT INNER JOIN
                         dbo.HoSoTau ON dbo.DIC_DEPARTMENT.DepartmentID = dbo.HoSoTau.DepartmentID INNER JOIN
                         dbo.ChungChiTau ON dbo.HoSoTau.ChungChiID = dbo.ChungChiTau.ChungChiID
 

END
go
---store dùng để update STT của hồ sơ công ty
create procedure sp_T_UpdateSTTLenXuong
(
	@id int, 
	@isLen int

)
as
	begin
		--lấy ra STT hiện tại của nó
		declare @sttHienTai int
		select @sttHienTai = STT
		from HoSoCongTy
		where HoSoCongTyID = @id

		--tìm ra số STT lớn nhất mà gần nó nhất
		declare @sttGanNhat int
		set @sttGanNhat = @sttHienTai
		if @isLen =1
			begin
				if (exists (select * from HoSoCongTy where STT<@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from HoSoCongTy
											where STT<@sttHienTai
											order by STT desc
										  )
					end
				end
		else
			begin
				if (exists (select * from HoSoCongTy where STT>@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from HoSoCongTy
											where STT>@sttHienTai
											order by STT asc
										  )
					end
			end		
		if (@sttGanNhat<> @sttHienTai)
			begin				
				--cập nhật lại 2 số STT
				update HoSoCongTy
				set STT = @sttHienTai
				where STT = @sttGanNhat
			
				update HoSoCongTy
				set STT = @sttGanNhat
				where HoSoCongTyID = @id
			end
		return
	end
go
---store dùng để update STT của chứng chỉ tàu
create procedure sp_T_UpdateSTTLenXuong_ChungChiTau
(
	@id int, 
	@isLen int

)
as
	begin
		--lấy ra STT hiện tại của nó
		declare @sttHienTai int
		select @sttHienTai = STT
		from ChungChiTau
		where ChungChiID = @id

		--tìm ra số STT lớn nhất mà gần nó nhất
		declare @sttGanNhat int
		set @sttGanNhat = @sttHienTai
		if @isLen =1
			begin
				if (exists (select * from ChungChiTau where STT<@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from ChungChiTau
											where STT<@sttHienTai
											order by STT desc
										  )
					end
				end
		else
			begin
				if (exists (select * from ChungChiTau where STT>@sttHienTai))
					begin
						set @sttGanNhat = (select top 1 STT
											from ChungChiTau
											where STT>@sttHienTai
											order by STT asc
										  )
					end
			end		
		if (@sttGanNhat<> @sttHienTai)
			begin				
				--cập nhật lại 2 số STT
				update ChungChiTau
				set STT = @sttHienTai
				where STT = @sttGanNhat
			
				update ChungChiTau
				set STT = @sttGanNhat
				where ChungChiID = @id
			end
		return
	end