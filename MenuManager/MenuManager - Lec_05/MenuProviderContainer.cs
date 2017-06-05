using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManager
{
    /// <summary>
    /// 컨테이너 클래스(중간 매개변수 역할)
    /// - InMemory, Xml, Sql인스턴스를 주입하여 각각의 모든 메뉴 리스트 반환
    /// </summary>
    public class MenuProviderContainer
    {
        private IMenuBase _repository; //_provider, _injector등을 많이 쓴다

        public MenuProviderContainer(IMenuBase provider) //보통 매개변수는 injector, factory, provider등 많이 쓴다
        {
            _repository = provider;
        }

        public List<Menu> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
