if not exists (select * from DIC_DEPARTMENT where DepartmentName like N'%Nghỉ có lương%')
	begin
		insert into DIC_DEPARTMENT(DepartmentName, Description, IsLast, ParentID) values (N'Nghỉ có lương', N'Nghỉ có lương', 1,17)
	end
go
---------------update view, cho phép thêm và hiển thị tàu ngoài
ALTER VIEW [dbo].[view_quatrinhcongtacFull]
AS
SELECT        dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, 
                         dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID, dbo.HRM_EMPLOYMENTHISTORY.DecisionNo, dbo.HRM_EMPLOYMENTHISTORY.DecisionDate, dbo.HRM_EMPLOYMENTHISTORY.ContentDecision, 
                         dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HRM_EMPLOYMENTHISTORY.PositionID, 
                         dbo.DIC_POSITION.PositionName, dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition, dbo.HRM_EMPLOYMENTHISTORY.PluralityID, DIC_POSITION_1.PositionName AS PluralityName, 
                         dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality, dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, 
                         dbo.fc_GetPositionByHRM_History(dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID) AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, 
                         dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, dbo.HRM_EMPLOYMENTHISTORY.EffectiveDate, dbo.HRM_EMPLOYMENTHISTORY.Note, 
                         dbo.HRM_EMPLOYMENTHISTORY.PerPosition, dbo.HRM_EMPLOYMENTHISTORY.PerPlurality, dbo.HRM_EMPLOYMENTHISTORY.SalaryPositionID, dbo.HRM_EMPLOYMENTHISTORY.SalaryPluralityID, 
                         dbo.HRM_EMPLOYMENTHISTORY.DepartmentName AS Expr1, dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID, dbo.HRM_EMPLOYMENTHISTORY.Gross, dbo.HRM_EMPLOYMENTHISTORY.Power, 
                         dbo.HRM_EMPLOYMENTHISTORY.NgayXuongTau, dbo.HRM_EMPLOYMENTHISTORY.XacNhan, dbo.HRM_EMPLOYEE.CardNo, dbo.HRM_EMPLOYEE.MainAddress, dbo.HRM_EMPLOYEE.CellPhone, 
                         dbo.HRM_EMPLOYEE.HomePhone, dbo.HRM_EMPLOYEE.IDCard, dbo.HRM_EMPLOYEE.IDCardDate, dbo.HRM_EMPLOYEE.IDCardPlace, dbo.HRM_EMPLOYEE.TaxNo, dbo.HRM_EMPLOYEE.BankCode, 
                         dbo.HRM_EMPLOYEE.BankName, dbo.HRM_EMPLOYEE.Qualification, dbo.HRM_EMPLOYEE.QuanHeID, dbo.HRM_EMPLOYEE.Note AS Expr2
FROM            dbo.HRM_EMPLOYMENTHISTORY INNER JOIN
                         dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                         dbo.DIC_DEPARTMENT ON dbo.HRM_EMPLOYMENTHISTORY.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP ON dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                         dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                         dbo.DIC_POSITION AS DIC_POSITION_1 ON dbo.HRM_EMPLOYMENTHISTORY.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                         dbo.DIC_POSITION ON dbo.HRM_EMPLOYMENTHISTORY.PositionID = dbo.DIC_POSITION.PositionID

GO
-------------------------------------------------------------
ALTER VIEW [dbo].[view_quatrinhcongtac]
AS
SELECT        dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, 
                         dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID, dbo.HRM_EMPLOYMENTHISTORY.DecisionNo, dbo.HRM_EMPLOYMENTHISTORY.DecisionDate, dbo.HRM_EMPLOYMENTHISTORY.ContentDecision, 
                         dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID, dbo.HRM_EMPLOYMENTHISTORY.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HRM_EMPLOYMENTHISTORY.PositionID, 
                         dbo.DIC_POSITION.PositionName, dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition, dbo.HRM_EMPLOYMENTHISTORY.PluralityID, DIC_POSITION_1.PositionName AS PluralityName, 
                         dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality, dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, 
                         dbo.fc_GetPositionByHRM_History(dbo.HRM_EMPLOYMENTHISTORY.EmploymentHistoryID) AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, 
                         dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, dbo.HRM_EMPLOYMENTHISTORY.EffectiveDate, dbo.HRM_EMPLOYMENTHISTORY.Note, 
                         dbo.HRM_EMPLOYMENTHISTORY.PerPosition, dbo.HRM_EMPLOYMENTHISTORY.PerPlurality, dbo.HRM_EMPLOYMENTHISTORY.SalaryPositionID, dbo.HRM_EMPLOYMENTHISTORY.SalaryPluralityID, 
                         dbo.HRM_EMPLOYMENTHISTORY.DepartmentName AS Expr1, dbo.HRM_EMPLOYMENTHISTORY.LoaiTauID, dbo.HRM_EMPLOYMENTHISTORY.Gross, dbo.HRM_EMPLOYMENTHISTORY.Power, 
                         dbo.HRM_EMPLOYMENTHISTORY.NgayXuongTau, dbo.HRM_EMPLOYMENTHISTORY.XacNhan, dbo.DIC_CATEGORYDECISION.CategoryDecisionName
FROM            dbo.HRM_EMPLOYMENTHISTORY INNER JOIN
                         dbo.HRM_EMPLOYEE ON dbo.HRM_EMPLOYMENTHISTORY.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                         dbo.DIC_DEPARTMENT ON dbo.HRM_EMPLOYMENTHISTORY.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                         dbo.DIC_CATEGORYDECISION ON dbo.HRM_EMPLOYMENTHISTORY.CategoryDecisionID = dbo.DIC_CATEGORYDECISION.CategoryDecisionID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP ON dbo.HRM_EMPLOYMENTHISTORY.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON dbo.HRM_EMPLOYMENTHISTORY.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                         dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                         dbo.DIC_POSITION AS DIC_POSITION_1 ON dbo.HRM_EMPLOYMENTHISTORY.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                         dbo.DIC_POSITION ON dbo.HRM_EMPLOYMENTHISTORY.PositionID = dbo.DIC_POSITION.PositionID
WHERE        (dbo.HRM_EMPLOYMENTHISTORY.XacNhan = 1)

GO
-----------------------CÁC HÀM TÍNH CREW MATRIX---------------------
-------------------WATCH KEEPING--------------------------------
CREATE function [dbo].[fc_T_IsSyQuan_WatchKeeping]
(
	@idChucDanh int
)
returns bit
as
	begin
		declare @kq bit, @nhomChucDanh int
		set @kq = 0
		--lấy ra nhóm chức danh, nếu là sỹ quan và khác thuyền trưởng và máy trưởng thì OK
		if (@idChucDanh <> 20 and @idChucDanh <> 24)
			begin
				select @nhomChucDanh = GroupPositionID
				from DIC_POSITION
				where PositionID = @idChucDanh
				if (@nhomChucDanh =1 or @nhomChucDanh =2)
					begin
						set @kq=1
					end 
			end		
		return @kq
	end
GO
CREATE function [dbo].[fc_T_CrewMatrix_WatchKeeping]
(
	@idEmployee int
)
returns int
as 
	begin
		declare @kq int
		set @kq =0
		if (@idEmployee>0)
			begin
				declare @ngayLenTau smalldatetime, @ngayRoiTau smalldatetime, @ngayHieuLuc smalldatetime, @department int,
				@chucDanh int, @loaiQuyetDinh int, @loaiTau int
				declare @capNhatNgay int --thông số cho biết có cần cập nhật lại ngày lên tàu
				set @ngayLenTau ='1900-01-01'
				set @ngayRoiTau ='1900-01-01'
				set @capNhatNgay =1
				DECLARE history_cursor CURSOR FOR   
						SELECT  DepartmentID, NgayXuongTau, PositionID, CategoryDecisionID, LoaiTauID
						FROM HRM_EMPLOYMENTHISTORY
						WHERE EmployeeID = @idEmployee and XacNhan=1 and NgayXuongTau is not null
						ORDER BY NgayXuongTau  
				OPEN history_cursor  

				FETCH NEXT FROM history_cursor   
				INTO @department, @ngayHieuLuc, @chucDanh, @loaiQuyetDinh, @loaiTau

				WHILE @@FETCH_STATUS = 0 
					begin
					--không phải là quyết định thôi việc và department là tàu thì tính theo ngày xuống tàu 
					if (@loaiQuyetDinh<>3 
						and (
							@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
							or (@department is null and @loaiTau is not null)
						)
						--các chức danh sỹ quan trừ thuyền trưởng và máy trướng
						and (dbo.fc_T_IsSyQuan_WatchKeeping(@chucDanh) =1)
					)
						begin
							if (@capNhatNgay =1)
								begin
									set @ngayLenTau = @ngayHieuLuc
									set @capNhatNgay =0
								end							
						end
					else
						begin
							if (@capNhatNgay =0)
								begin
									set @ngayRoiTau = @ngayHieuLuc
									set @kq = @kq + DATEDIFF(day, @ngayLenTau, @ngayRoiTau)
									set @capNhatNgay =1
								end
						end		
					
					FETCH NEXT FROM history_cursor   
					INTO @department, @ngayHieuLuc, @chucDanh, @loaiQuyetDinh, @loaiTau
					end
				CLOSE history_cursor;  
				DEALLOCATE history_cursor;
				if((@ngayLenTau<>'1900-01-01') and (@ngayRoiTau='1900-01-01' or @ngayRoiTau<@ngayLenTau))
					begin
						--lấy ngày hiện tại
						set @kq = @kq+ DATEDIFF(day, @ngayLenTau, GETDATE())
					end
			end
		
		return @kq
	end

GO
---------------WITH OPERATOR-----------------
CREATE function [dbo].[fc_T_CrewMatrix_WithOperator]
(
	@idEmployee int
)
returns int
as 
	begin
		declare @kq int
		set @kq =0
		if (@idEmployee>0)
			begin
				declare @ngayQuyetDinh smalldatetime, @loaiQuyetDinh int, @ngayVaoLam smalldatetime, @ngayNghiViec smalldatetime
				declare @capNhatNgay int --thông số cho biết có cần cập nhật lại ngày vào làm
				set @ngayVaoLam ='1900-01-01'
				set @ngayNghiViec ='1900-01-01'
				DECLARE history_cursor CURSOR FOR 
						select NgayHieuLuc, CategoryDecisionID as LoaiQuyetDinh
						from (
							select ContractDate as NgayHieuLuc, 2 as CategoryDecisionID
							from HRM_CONTRACTHISTORY
							where EmployeeID = @idEmployee and ContractDate is not null
							union all
							--lấy ra ngày thanh lý hợp đồng cho nghỉ
							SELECT  EffectiveDate as NgayHieuLuc, 3 as CategoryDecisionID
							FROM HRM_EMPLOYMENTHISTORY
							WHERE EmployeeID = @idEmployee and CategoryDecisionID =3 and EffectiveDate is not null
						) as t
						Order by NgayHieuLuc								
				OPEN history_cursor  

				FETCH NEXT FROM history_cursor   
				INTO @ngayQuyetDinh, @loaiQuyetDinh

				WHILE @@FETCH_STATUS = 0 
					begin
					--nếu là quyết định ký hợp đồng 
					if (@loaiQuyetDinh=2)
						begin
							set @ngayVaoLam = @ngayQuyetDinh
						end 
					if (@loaiQuyetDinh =3) --thanh lý hợp đồng
						begin
							set @ngayNghiViec = @ngayQuyetDinh
						end
					--bắt đầu tính ngày
					if (@ngayVaoLam<>'1900-01-01' and @ngayNghiViec<>'1900-01-01')
						begin
							if(@ngayNghiViec>@ngayVaoLam)
							begin
								set @kq = @kq + DATEDIFF(day, @ngayVaoLam, @ngayNghiViec)
							end
						end
					FETCH NEXT FROM history_cursor   
					INTO @ngayQuyetDinh, @loaiQuyetDinh
					end
				CLOSE history_cursor;  
				DEALLOCATE history_cursor;
				if((@ngayVaoLam<>'1900-01-01') and (@ngayNghiViec='1900-01-01' or @ngayNghiViec<@ngayVaoLam))
					begin
						--lấy ngày hiện tại
						set @kq = @kq+ DATEDIFF(day, @ngayVaoLam, GETDATE())
					end
			end
		
		return @kq
	end

GO
----------------IN RANK------------------------
CREATE function [dbo].[fc_T_CrewMatrix_InRank]
(
	@idViTri int,
	@idEmployee int
)
returns int
as 
	begin
		declare @kq int
		set @kq =0
		if (@idEmployee>0 and @idViTri>0)
		begin
			declare @ngayLenTau smalldatetime, @ngayRoiTau smalldatetime, @ngayHieuLuc smalldatetime, @department int, @loaiTau int,
			@chucDanh int, @chucDanhKiemNhiem int, @loaiQuyetDinh int
			declare @flag int
			set @flag = 0
			set @ngayLenTau ='1900-01-01'
			DECLARE history_cursor CURSOR FOR   
						SELECT  DepartmentID, NgayXuongTau, PositionID, PluralityID, CategoryDecisionID, LoaiTauID 
						FROM HRM_EMPLOYMENTHISTORY
						WHERE EmployeeID = @idEmployee and XacNhan=1 and NgayXuongTau is not null
						ORDER BY NgayXuongTau  
			OPEN history_cursor  

			FETCH NEXT FROM history_cursor   
			INTO @department, @ngayHieuLuc, @chucDanh, @chucDanhKiemNhiem, @loaiQuyetDinh, @loaiTau

			WHILE @@FETCH_STATUS = 0 
				begin
				if (@loaiQuyetDinh<>3 
					and (
							@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
							or (@department is null and (@loaiTau=1 or @loaiTau=2))--tàu dầu hay hóa chất	
					) 
					and (@chucDanh =@idViTri or @chucDanhKiemNhiem = @idViTri)
				)
					begin
						if (@flag = 0)
							begin
								set @flag = 1
								set @ngayLenTau = @ngayHieuLuc
							end
					end
				else
					if (@flag =1)
						begin
							set @flag = 0
							set @ngayRoiTau = @ngayHieuLuc
							set @kq = @kq + DATEDIFF(day, @ngayLenTau, @ngayRoiTau)
					end 

				FETCH NEXT FROM history_cursor   
				INTO @department, @ngayHieuLuc, @chucDanh, @chucDanhKiemNhiem, @loaiQuyetDinh, @loaiTau
				end
			CLOSE history_cursor;  

			DEALLOCATE history_cursor;
			
			if (@flag = 0)
				begin
					if (@loaiQuyetDinh<>3 
					and (
							@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
							or (@department is null and (@loaiTau=1 or @loaiTau=2))--tàu dầu hay hóa chất	
					) 
					and (@chucDanh =@idViTri or @chucDanhKiemNhiem = @idViTri)
					)			
					begin
						--lấy ngày hiện tại
						set @kq = @kq + DATEDIFF(day, @ngayHieuLuc, GETDATE())
					end
				end
			else
				begin
					if (@loaiQuyetDinh<>3 
					and (
							@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
							or (@department is null and (@loaiTau=1 or @loaiTau=2))--tàu dầu hay hóa chất	
					) 
					and (@chucDanh =@idViTri or @chucDanhKiemNhiem = @idViTri)
					)			
					begin
						--lấy ngày hiện tại
						set @kq = @kq + DATEDIFF(day, @ngayLenTau, GETDATE())
					end
				end

		end		
		return @kq
	end


GO
CREATE function [dbo].[fc_T_CrewMatrix_ThisTypeOfTanker]
(
	@idEmployee int
)
returns int
as 
	begin
		declare @kq int
		set @kq =0
		if (@idEmployee>0)
			begin
				declare @ngayLenTau smalldatetime, @ngayRoiTau smalldatetime, @ngayHieuLuc smalldatetime, @department int, @loaiTau int,
				@loaiQuyetDinh int
				declare @capNhatNgay int --thông số cho biết có cần cập nhật lại ngày lên tàu
				set @ngayLenTau ='1900-01-01'
				set @ngayRoiTau ='1900-01-01'
				set @capNhatNgay =1
				DECLARE history_cursor CURSOR FOR   
						SELECT  DepartmentID, NgayXuongTau, CategoryDecisionID, LoaiTauID 
						FROM HRM_EMPLOYMENTHISTORY
						WHERE EmployeeID = @idEmployee and XacNhan=1 and NgayXuongTau is not null
						ORDER BY NgayXuongTau  
				OPEN history_cursor  

				FETCH NEXT FROM history_cursor   
				INTO @department, @ngayHieuLuc, @loaiQuyetDinh, @loaiTau

				WHILE @@FETCH_STATUS = 0 
					begin
						--không phải là quyết định thôi việc và department là tàu thì tính theo ngày xuống tàu 
						if (@loaiQuyetDinh<>3 
							and (
								@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
								or (@department is null and (@loaiTau=1 or @loaiTau=2))--chỉ không tính tàu hàng
							) 
						)
							begin
								if (@capNhatNgay =1)
									begin
										set @ngayLenTau = @ngayHieuLuc
										set @capNhatNgay =0
									end								
							end 
						else
							begin
								if (@capNhatNgay =0)
									begin
										set @ngayRoiTau = @ngayHieuLuc
										set @kq = @kq + DATEDIFF(day, @ngayLenTau, @ngayRoiTau)
										set @capNhatNgay =1
									end								
							end				
						FETCH NEXT FROM history_cursor   
						INTO @department, @ngayHieuLuc, @loaiQuyetDinh, @loaiTau
					end
				CLOSE history_cursor;  
				DEALLOCATE history_cursor;
				if((@ngayLenTau<>'1900-01-01') and (@ngayRoiTau='1900-01-01' or @ngayRoiTau<@ngayLenTau))
					begin
						--lấy ngày hiện tại
						set @kq = @kq+ DATEDIFF(day, @ngayLenTau, GETDATE())
					end
			end
		
		return @kq
	end

GO

---------------------------ALL TYPE OF TANKER----------------------
CREATE function [dbo].[fc_T_CrewMatrix_AllTypeOfTanker]
(
	@idEmployee int
)
returns int
as 
	begin
		declare @kq int
		set @kq =0
		if (@idEmployee>0)
			begin
				declare @ngayLenTau smalldatetime, @ngayRoiTau smalldatetime, @ngayHieuLuc smalldatetime, @department int, @loaiTau int,
				@loaiQuyetDinh int
				declare @capNhatNgay int --thông số cho biết có cần cập nhật lại ngày lên tàu
				set @ngayLenTau ='1900-01-01'
				set @ngayRoiTau ='1900-01-01'
				set @capNhatNgay =1
				DECLARE history_cursor CURSOR FOR   
						SELECT  DepartmentID, NgayXuongTau, CategoryDecisionID, LoaiTauID 
						FROM HRM_EMPLOYMENTHISTORY
						WHERE EmployeeID = @idEmployee and XacNhan=1 and NgayXuongTau is not null
						ORDER BY NgayXuongTau  
				OPEN history_cursor  

				FETCH NEXT FROM history_cursor   
				INTO @department, @ngayHieuLuc, @loaiQuyetDinh, @loaiTau

				WHILE @@FETCH_STATUS = 0 
					begin
					--không phải là quyết định thôi việc và department là tàu thì tính theo ngày xuống tàu 
					if (@loaiQuyetDinh<>3 
						and (
							@department in (select DepartmentID from DIC_DEPARTMENT where ParentID=8)
							or (@department is null and @loaiTau<>5 and @loaiTau is not null)--chỉ không tính tàu hàng
						) 
					)
						begin
							if (@capNhatNgay=1)
								begin
									set @ngayLenTau = @ngayHieuLuc
									set @capNhatNgay =0
								end							
						end 
					else
						begin
							if (@capNhatNgay=0)
								begin
									set @ngayRoiTau = @ngayHieuLuc
									set @kq = @kq + DATEDIFF(day, @ngayLenTau, @ngayRoiTau)
									set @capNhatNgay =1
								end	
						end					
					FETCH NEXT FROM history_cursor   
					INTO @department, @ngayHieuLuc, @loaiQuyetDinh, @loaiTau
					end
				CLOSE history_cursor;  
				DEALLOCATE history_cursor;
				if((@ngayLenTau<>'1900-01-01') and (@ngayRoiTau='1900-01-01' or @ngayRoiTau<@ngayLenTau))
					begin
						--lấy ngày hiện tại
						set @kq = @kq+ DATEDIFF(day, @ngayLenTau, GETDATE())
					end
			end
		
		return @kq
	end

GO






