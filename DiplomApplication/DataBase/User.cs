using Avalonia.Media.Imaging;
using DiplomApplication.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.DataBase;

public partial class User
{
    public int UserId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public uint? BonusCount { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Password { get; set; } = null!;

    public string? Userimage { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public Bitmap UserPhoto => Userimage != null ? new Bitmap(Userimage.Trim()) : new Bitmap(".\\Users\\user.png");
    public string UserRole
    {
        get
        {
            var currentPosition = DataBaseClass.DataBase.Roles.First(f => f.RoleId == RoleId);
            return currentPosition.Title;
        }
    }

}
