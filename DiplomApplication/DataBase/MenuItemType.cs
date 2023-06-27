using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class MenuItemType
{
    public int MenuItemTypeId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; } = new List<Menu>();
}
