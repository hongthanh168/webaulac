USE [AuLac]
GO

/****** Object:  View [dbo].[view_qtct_goc]    Script Date: 11/26/2018 11:50:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[view_qtct_goc]
AS
SELECT        dbo.HRM_EMPLOYEE.EmployeeID, dbo.HRM_EMPLOYEE.EmployeeCode, dbo.HRM_EMPLOYEE.FirstName, dbo.HRM_EMPLOYEE.LastName, dbo.HRM_EMPLOYEE.BirthDay, dbo.HRM_EMPLOYEE.BirthPlace, 
                         dbo.HRM_QTCT.EmploymentHistoryID, dbo.HRM_QTCT.QTCTID, dbo.HRM_QTCT.DecisionNo, dbo.HRM_QTCT.DecisionDate, dbo.HRM_QTCT.ContentDecision, dbo.HRM_QTCT.CategoryDecisionID, 
                         dbo.HRM_QTCT.DepartmentID, dbo.DIC_DEPARTMENT.DepartmentName, dbo.HRM_QTCT.PositionID, dbo.DIC_POSITION.PositionName, dbo.HRM_QTCT.InternshipPosition, dbo.HRM_QTCT.PluralityID, 
                         DIC_POSITION_1.PositionName AS PluralityName, dbo.HRM_QTCT.IntershipPlurality, dbo.DIC_EDUCATION.EducationName, dbo.DIC_EDUCATION.Description AS EducationDescription, dbo.DIC_EDUCATION.EducationID, 
                         dbo.fc_GetPositionByHRM_History(dbo.HRM_QTCT.EmploymentHistoryID) AS ChucVu, dbo.HRM_EMPLOYEE.ContactAddress, dbo.DIC_DEPARTMENT.Description AS DepartmentDescription, 
                         dbo.DIC_INTERSHIP.IntershipName AS IntershipPositionName, DIC_INTERSHIP_1.IntershipName AS IntershipPluralityName, dbo.HRM_QTCT.EffectiveDate, dbo.HRM_QTCT.Note, dbo.HRM_QTCT.PerPosition, 
                         dbo.HRM_QTCT.PerPlurality, dbo.HRM_QTCT.SalaryPositionID, dbo.HRM_QTCT.SalaryPluralityID, dbo.HRM_QTCT.DepartmentName AS Expr1, dbo.HRM_QTCT.LoaiTauID, dbo.HRM_QTCT.Gross, dbo.HRM_QTCT.Power, 
                         dbo.HRM_QTCT.NgayXuongTau, dbo.HRM_QTCT.XacNhan, dbo.DIC_CATEGORYDECISION.CategoryDecisionName
FROM            dbo.HRM_QTCT INNER JOIN
                         dbo.HRM_EMPLOYEE ON dbo.HRM_QTCT.EmployeeID = dbo.HRM_EMPLOYEE.EmployeeID LEFT OUTER JOIN
                         dbo.DIC_DEPARTMENT ON dbo.HRM_QTCT.DepartmentID = dbo.DIC_DEPARTMENT.DepartmentID LEFT OUTER JOIN
                         dbo.DIC_CATEGORYDECISION ON dbo.HRM_QTCT.CategoryDecisionID = dbo.DIC_CATEGORYDECISION.CategoryDecisionID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP ON dbo.HRM_QTCT.InternshipPosition = dbo.DIC_INTERSHIP.IntershipID LEFT OUTER JOIN
                         dbo.DIC_INTERSHIP AS DIC_INTERSHIP_1 ON dbo.HRM_QTCT.IntershipPlurality = DIC_INTERSHIP_1.IntershipID LEFT OUTER JOIN
                         dbo.DIC_EDUCATION ON dbo.HRM_EMPLOYEE.EducationID = dbo.DIC_EDUCATION.EducationID LEFT OUTER JOIN
                         dbo.DIC_POSITION AS DIC_POSITION_1 ON dbo.HRM_QTCT.PluralityID = DIC_POSITION_1.PositionID LEFT OUTER JOIN
                         dbo.DIC_POSITION ON dbo.HRM_QTCT.PositionID = dbo.DIC_POSITION.PositionID
WHERE        (dbo.HRM_QTCT.XacNhan = 1)

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "HRM_QTCT"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "HRM_EMPLOYEE"
            Begin Extent = 
               Top = 6
               Left = 296
               Bottom = 136
               Right = 509
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_DEPARTMENT"
            Begin Extent = 
               Top = 6
               Left = 547
               Bottom = 136
               Right = 746
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_CATEGORYDECISION"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 234
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_INTERSHIP"
            Begin Extent = 
               Top = 138
               Left = 290
               Bottom = 234
               Right = 460
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_INTERSHIP_1"
            Begin Extent = 
               Top = 138
               Left = 498
               Bottom = 234
               Right = 668
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_EDUCATION"
            Begin Extent = 
               Top = 234
               Left = 38
               Bottom = 347
               Right = 212
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_qtct_goc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'        End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_POSITION_1"
            Begin Extent = 
               Top = 234
               Left = 250
               Bottom = 364
               Right = 433
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIC_POSITION"
            Begin Extent = 
               Top = 234
               Left = 471
               Bottom = 364
               Right = 654
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_qtct_goc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_qtct_goc'
GO


