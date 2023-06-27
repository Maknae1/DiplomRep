using DiplomApplication.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.DataBase;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderDate { get; set; }

    public int UserId { get; set; }

    public decimal Cost { get; set; }

    public int PboId { get; set; }

    public string SecurityCode { get; set; } = null!;

    public virtual Pbo Pbo { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public string UserName
    {
        get
        {
            var currentUser = DataBaseClass.DataBase.Users.First(f => f.UserId == UserId);
            return $"{currentUser.FirstName} {currentUser.LastName} {currentUser.Patronymic}";
        }
    }
}
