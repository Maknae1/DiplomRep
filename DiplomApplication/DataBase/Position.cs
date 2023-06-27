using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Position
{
    public int PositionId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
