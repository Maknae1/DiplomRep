using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Product
{
    public int ProductsId { get; set; }

    public string Title { get; set; } = null!;

    public int Proteins { get; set; }

    public int Fats { get; set; }

    public int Carbohydrates { get; set; }

    public int Calories { get; set; }
}
