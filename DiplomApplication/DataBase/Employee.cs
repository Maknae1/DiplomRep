using Avalonia.Media.Imaging;
using DiplomApplication.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.DataBase;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public long Inn { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int PassportNumber { get; set; }

    public int PassportSeries { get; set; }

    public long LaborContractNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int PositionId { get; set; }

    public int PboId { get; set; }

    public string? Image { get; set; }

    public virtual Pbo Pbo { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;

    public Bitmap EmployeeImage => Image != null ? new Bitmap(Image.Trim()) : new Bitmap(".\\Employees\\user.png");
    public string EmployeePosition 
    {
        get
        {
            var currentPosition = DataBaseClass.DataBase.Positions.First(f => f.PositionId == PositionId);
            return currentPosition.Title;
        }
    }
}
