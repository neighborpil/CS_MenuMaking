using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
/*
[메뉴 매니저]

#메뉴 구성
 - 사이드 메뉴
    책
        좋은책
        나쁜책
    강의
        좋은강의
        나쁜강의
    컴퓨터
        데스크톱
        노트북

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

*/
namespace MenuManager
{
    /// <summary>
    /// 메뉴(Menu) 모델 클래스 : Menu, MenuModel, MenuViewModel, ...
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 일련번호 + 고유키
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 보여지는 순서
        /// </summary>
        public int MenuOrder { get; set; }
                
        /// <summary>
        /// //부모 메뉴 번호 : 최상위 (0)
        /// C# 6.0에서는 속성에 바로 값을 지정 할 수 있다
        /// </summary>
        public int ParentId { get; set; } = 0;  

        /// <summary>
        /// 메뉴 이름
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 선택할 때 이동 경로
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// (자식) 메뉴 표시 여부
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// 자식 메뉴 리스트
        /// </summary>
        public List<Menu> Menus { get; set; } = new List<Menu>();

        public override string ToString()
        {
            return MenuName;
        }
    }

    /// <summary>
    /// 메뉴 관리자의 부모 클래스 => 추상 클래스로 구현 => 나중에 인터페이스로
    /// </summary>
    public abstract class MenuBase
    {
        public abstract List<Menu> GetAll(); //메뉴베이스를 상속 받는 모든 클래스는 메뉴 반환 메소드 생성해라
    }

    public class MenuDataInMemory : MenuBase
    {
        public override List<Menu> GetAll()
        {
            List<Menu> menus = new List<Menu>()
            {
                new Menu{ MenuId = 1, MenuName = "책" },
                new Menu{ MenuId = 2, MenuName = "강의" },
                new Menu{ MenuId = 3, MenuName = "컴퓨터" }
            };

            return menus;
        }
    }

    public class MenuDataInXml : MenuBase
    {
        public override List<Menu> GetAll()
        {
            List<Menu> menus = new List<Menu>()
            {
                new Menu{ MenuId = 11, MenuName = "XML책" },
                new Menu{ MenuId = 12, MenuName = "XML강의" },
                new Menu{ MenuId = 13, MenuName = "XML컴퓨터" }
            };

            return menus;
        }
    }

    public class MenuDataInSql : MenuBase
    {
        public override List<Menu> GetAll()
        {
            List<Menu> menus = new List<Menu>()
            {
                new Menu{ MenuId = 21, MenuName = "SQL책" },
                new Menu{ MenuId = 22, MenuName = "SQL강의" },
                new Menu{ MenuId = 23, MenuName = "SQL컴퓨터" }
            };

            return menus;
        }
    }

    /// <summary>
    /// 컨테이너 클래스(중간 매개변수 역할)
    /// - InMemory, Xml, Sql인스턴스를 주입하여 각각의 모든 메뉴 리스트 반환
    /// </summary>
    public class MenuProviderContainer
    {
        private MenuBase _repository; //_provider, _injector등을 많이 쓴다

        public MenuProviderContainer(MenuBase provider) //보통 매개변수는 injector, factory, provider등 많이 쓴다
        {
            _repository = provider;
        }

        public List<Menu> GetAll()
        {
            return _repository.GetAll();
        }
    }

    public class MenuManager
    {
        static void Main(string[] args)
        {
            #region Menu 모델 클래스 테스트

            Menu m1 = new Menu();

            m1.MenuId = 1;
            m1.MenuName = "책";
            m1.MenuPath = "/Home";
            WriteLine($"{m1.MenuId} - {m1.MenuName}");

            WriteLine();

            List<Menu> m2 = new List<Menu>()
            {
                new Menu{ MenuId = 1, MenuName = "책" },
                new Menu{ MenuId = 2, MenuName = "강의" },
                new Menu{ MenuId = 3, MenuName = "컴퓨터" }
            };


            foreach (var m in m2)
            {
                WriteLine($"{m.MenuId} - {m.MenuName}");
            }

            WriteLine();

            #endregion


            #region 인 - 메모리 데이터 사용 출력

            //인 - 메모리 데이터 사용 출력
            MenuDataInMemory inMemory = new MenuDataInMemory();
            foreach (var i in inMemory.GetAll())
            {
                WriteLine($"{i.MenuId} - {i.MenuName}");
            }

            WriteLine();

            #endregion

            #region 컨테이너 클래스를 통하여 개체를 생성

            //컨테이너 클래스를 통하여 개체를 생성
            //InMemory
            var container = new MenuProviderContainer(new MenuDataInMemory()); //메뉴 클래스를 주입
            foreach (var c in inMemory.GetAll())
            {
                WriteLine($"{c.MenuId} - {c.MenuName}");
            }

            WriteLine();

            //Xml
            var xml = new MenuProviderContainer(new MenuDataInXml());
            foreach (var x in xml.GetAll())
            {
                WriteLine($"{x.MenuId} - {x.MenuName}");
            }
             
            WriteLine();

            //SQL
            var sql = new MenuProviderContainer(new MenuDataInSql());
            foreach (var s in sql.GetAll())
            {
                WriteLine($"{s.MenuId} - {s.MenuName}");
            }
            #endregion



#if DEBUG
            ReadKey();
#endif
        }
    }
}
