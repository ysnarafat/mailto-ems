using System.Collections.Generic;

namespace EmailMarketing.Framework.Menus
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public IList<MenuChildItem> Children { get; set; }
    }
}
