using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Menu
{
    public int MenuItemId { get; set; }

    public string Title { get; set; } = null!;

    public int MenuItemTypeId { get; set; }

    public decimal Cost { get; set; }

    public string Composition { get; set; } = null!;

    public string? Image { get; set; }

    public int Calories { get; set; }

    public int Energyvalue { get; set; }

    public int Proteins { get; set; }

    public int Fats { get; set; }

    public int Carbohydrates { get; set; }

    public string? Description { get; set; }

    public virtual MenuItemType MenuItemType { get; set; } = null!;

    public Bitmap MenuItemImage => Image != null ? new Bitmap(Image.Trim()) : new Bitmap(".\\Employees\\logo.png");
    public uint Count;

}
