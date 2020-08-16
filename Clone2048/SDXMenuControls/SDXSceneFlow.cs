using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clone2048.SDXMenuControls
{
    public class SDXSceneFlow
    {
        public List<SDXMenu> menuList;
        public int activeMenu;
        public bool isVisible;
        public SDXSceneFlow()
        {
            menuList = new List<SDXMenu>();
            activeMenu = 0;
            isVisible = true;
        }

        public int NextMenu(string next)
        {
            foreach(SDXMenu menu in menuList)
            {
                if (menu.menuName == next)
                {
                    activeMenu= menuList.IndexOf(menu);
                    return activeMenu;
                }
            }            
            return -1;
        }
    }
}
