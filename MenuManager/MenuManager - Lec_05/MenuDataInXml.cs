using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MenuManager
{

    public class MenuDataInXml : IMenuBase
    {
        private string _connectionString;

        public MenuDataInXml(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Menu> GetAll()
        {
            //App_Data\\Menus.xml 파일 로드
            XElement xml = XElement.Load(_connectionString);


            return GetMenuData(xml, 0);
        }

        /// <summary>
        /// XML의 내용을 읽어 들여 메뉴를 구성하는데, 자식메뉴도 사용하기 위하여
        /// 재귀 함수로 만들어 준다
        /// </summary>
        /// <param name="xml">xml파일</param>
        /// <param name="parentId">부모의 아이디(레벨을 나타냄)</param>
        /// <returns></returns>
        private List<Menu> GetMenuData(XElement xml, int parentId)
        {
            List<Menu> menus = new List<Menu>();
            var xmlMenu = (
                from node in xml.Elements("Menu")
                where Convert.ToInt32(node.Element("ParentId").Value) == parentId
                select new Menu
                {
                    MenuId = Convert.ToInt32(node.Element("MenuId").Value),
                    MenuName = node.Element("MenuName").Value,
                    //자식요소들은 재귀 함수를 사용하여 Menus에 채워 넣음
                    Menus = (parentId != Convert.ToInt32(node.Element("MenuId").Value))
                        ? GetMenuData(xml, Convert.ToInt32(node.Element("MenuId").Value)) : new List<Menu>()
                }
            );
            menus = xmlMenu.ToList();

            return menus;
        }
    }
}
