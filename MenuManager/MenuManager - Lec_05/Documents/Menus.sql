CREATE TABLE [dbo].[Menus]
(
	[MenuId] INT Identity(1, 1) NOT NULL PRIMARY KEY, /* Identity(1, 1) : 일련번호 */
	[MenuOrder] INT NOT NULL,
	
	[ParentId] INT DEFAULT(0), /* 0또는 null이면 최상단*/
	[MenuName] NVARCHAR(100) NOT NULL,
	[MenuPath] NVARCHAR(255) NULL,
	[IsVisible] BIT DEFAULT(1) NOT NULL
)
GO

SET IDENTITY_INSERT [dbo].[Menus] ON
INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible])
VALUES (1, 1, 0, N'책(SQL)', N'/Home/Book', 1)
INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) 
VALUES (2, 2, 0, N'강의(SQL)', N'/Home/Lecture', 1)
INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) 
VALUES (3, 3, 1, N'좋은책(SQL)', NULL, 1)
INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) 
VALUES (4, 4, 1, N'나쁜책(SQL)', NULL, 1)
SET IDENTITY_INSERT [dbo].[Menus] OFF
