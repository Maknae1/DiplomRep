using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Pbo
{
    public int PboId { get; set; }

    public string Title { get; set; } = null!;

    public int DirectorId { get; set; }

    public string Address { get; set; } = null!;

    public decimal? ThisYearProceeds { get; set; }

    public virtual Director Director { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Questionaire> Questionaires { get; } = new List<Questionaire>();
}
