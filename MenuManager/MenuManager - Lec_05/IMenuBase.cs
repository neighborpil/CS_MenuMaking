using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManager
{
    /// <summary>
    /// 메뉴 관리자의 부모 클래스 => 추상 클래스로 구현 => 나중에 인터페이스로
    /// </summary>
    public interface IMenuBase
    {
        List<Menu> GetAll(); //메뉴베이스를 상속 받는 모든 클래스는 메뉴 반환 메소드 생성해라
    }
}
