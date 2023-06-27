using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Director
{
    public int DirectorId { get; set; }

    public string Directorname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int Passportnumber { get; set; }

    public int Passportseries { get; set; }

    public string Laborcontractnumber { get; set; } = null!;

    public DateOnly Dateofbirth { get; set; }

    public virtual ICollection<Pbo> Pbos { get; } = new List<Pbo>();
}
