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
