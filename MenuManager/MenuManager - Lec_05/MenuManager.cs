using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Console;
/*
[메뉴 매니저]
#메뉴 데이터
 - 인 -메모리
 - XML
 - SQL

#구조
MenuBase => IMenuRepository, IMenuBase, ... //인터페이스로 구성
 -> MenuDataInMemory
 -> Menu DataSql
    -> MenuDataSqlServer
    -> MenuDataOracle
 -> MenuDataXml

MenuPoviderContainer(new MenuBase()) => 각각의 리파지터리 개체 생성

#MenuDataInSql
 - DB : 로컬 DB
 - 데이터베이스 : Manager
 - 테이블 : Menus

#로컬 SQL 서버 사용법
 1. [메뉴] - [보기] - [Sql Server Object Explorer]
 2. (localdb)\MSSQLLocalDB에서 메뉴 펼치기
 3. [Databases]에서 우클릭 - [Add New Database]
 4. MenuManger라는 이름의 DB하나 생성
 5. [MenuManger] DB를 펼쳐서 [Tables]항목에서 우클릭 [Add New Table] 클릭
 6. 아래 디자인 부분의 [T-SQL]부분에 쿼리문 작성
    CREATE TABLE [dbo].[Menus]
    (
    	[MenuId] INT Identity(1, 1) NOT NULL PRIMARY KEY, //Identity(1, 1) : 일련번호
	    [MenuOrder] INT NOT NULL,
	
	    [ParentId] INT DEFAULT(0), // 0또는 null이면 최상단
	    [MenuName] NVARCHAR(100) NOT NULL,
	    [MenuPath] NVARCHAR(255) NULL,
	    [IsVisible] BIT DEFAULT(1) NOT NULL
    )
    GO
 7. 작성한 쿼리문을 드래그하여 전체를 선택한 후 메뉴 상단의 [Update] 버튼을 클릭
 8. [Update Database]버튼 클릭하여 완료
 9. 다시 [메뉴] - [보기] - [Sql Server Object Explorer]를 클릭
 10. (localdb)\MSSQLLocalDB - MenuManger - Tables - dbo.Menus테이블이 있는지 확인
 11. sql문 복사한 뒤 Documents 폴더를 생성하여 텍스트 문서하나 생성(이름 : Menus.sql)
 12. 파일 내에 sql문 넣기(혹시 프로젝트에 포함되어 있지 않다면 우클릭하여 프로젝트에 포함)
 //데이터 넣기
 13. [메뉴] - [보기] - [Sql Server Object Explorer]를 클릭
 14. (localdb)\MSSQLLocalDB - MenuManger - Tables - dbo.Menus테이블 우클릭
 15. [View Data]항목 클릭
 16. 데이터를 입력하고 상단의 페이퍼 모양[Script]버튼 클릭하여 SQL문 보기, 복사
     SET IDENTITY_INSERT [dbo].[Menus] ON
     INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) VALUES (1, 1, 0, N'책(SQL)', N'/Home/Book', 1)
     INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) VALUES (2, 2, 0, N'강의(SQL)', N'/Home/Lecture', 1)
     INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) VALUES (3, 3, 1, N'좋은책(SQL)', NULL, 1)
     INSERT INTO [dbo].[Menus] ([MenuId], [MenuOrder], [ParentId], [MenuName], [MenuPath], [IsVisible]) VALUES (4, 4, 1, N'나쁜책(SQL)', NULL, 1)
     SET IDENTITY_INSERT [dbo].[Menus] OFF
 17. 재사용 위해서 Insert문을 Menus.sql파일 내에 복붙
 18. sql 코드 작성


*/
namespace MenuManager
{
    public class MenuManager
    {
        static void Main(string[] args)
        {
            #region [1]인-메모리 컬렉션 사용
            WriteLine("[1] 인-메모리 컬렉션 사용");

            //서브메뉴 출력
            var subMenu = new MenuProviderContainer(new MenuDataInMemory());
            MenuPrint(subMenu.GetAll());
            #endregion

            #region [2] XML 사용
            WriteLine("[2] XML 사용");

            //XML서브메뉴 출력
            var xmlMenu = new MenuProviderContainer(
                new MenuDataInXml(
                    Path.Combine(
                        Directory.GetCurrentDirectory(), "App_Data\\Menus.xml")
                    )
                );
            MenuPrint(xmlMenu.GetAll());
            #endregion

            #region [3] SQL Server 데이터베이스 사용
            WriteLine("[3] SQL Server 데이터베이스 사용");

            //SQL서브메뉴 출력
            var sqlMenu = new MenuProviderContainer(
                new MenuDataInSql("server=(localdb)\\mssqllocaldb;database=MenuManager;Integrated Security=True;"));
            MenuPrint(sqlMenu.GetAll());
            #endregion

#if DEBUG
            ReadKey();
#endif
        }

        /// <summary>
        /// 메뉴 프린트
        /// </summary>
        /// <param name="menus"></param>
        private static void MenuPrint(List<Menu> menus)
        {
            foreach (var menu in menus)
            {
                //부모 요소 출력
                WriteLine($"{menu.MenuId} - {menu.MenuName}");
                //자식 요소가 있으면
                if (menu.Menus.Count > 0)
                {
                    foreach (var c in menu.Menus)
                    {
                        WriteLine($"\t{c.MenuId} - {c.MenuName}");
                    }
                }
            }

            WriteLine();
        }
    }
}
