USE [AuLac]
GO


/****** Object:  Table [dbo].[HRM_USERROLES]    Script Date: 10/24/2018 9:34:58 AM ******/
DROP TABLE [dbo].[HRM_USERROLES]
GO

/****** Object:  Table [dbo].[HRM_USERROLES]    Script Date: 10/24/2018 9:34:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HRM_USERROLES](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_HRM_USERROLES] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


DROP TABLE [dbo].[HRM_ROLE]
GO

/****** Object:  Table [dbo].[HRM_ROLE]    Script Date: 10/24/2018 9:33:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HRM_ROLE](
	[RoleID] [nvarchar](128) NOT NULL,
	[RoleName] [nvarchar](100) NULL,
 CONSTRAINT [PK_HRM_ROLE] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'Admin', N'Admin')
INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'Boss', N'Giám đốc')
INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'Daotao', N'Đào tạo')
INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'EduCenter', N'Trung tâm thuyền viên')
INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'HR', N'Hành chính – Nhân sự')
INSERT [dbo].[HRM_ROLE] ([RoleID], [RoleName]) VALUES (N'View', N'View')

