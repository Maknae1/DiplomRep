using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class Questionaire
{
    public int QuestionairId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public int PboId { get; set; }

    public string? Questionairstatus { get; set; }

    public virtual Pbo Pbo { get; set; } = null!;

    public IBrush Color => Questionairstatus != "Ожидает" ? Brushes.AliceBlue : Brushes.PeachPuff;

}
