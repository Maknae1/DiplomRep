using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class FeedBack
{
    public int FeedbackId { get; set; }

    public int FeedbackRateId { get; set; }

    public string FeedBackText { get; set; } = null!;

    public int PboId { get; set; }

    public virtual FeedBackRate FeedbackRate { get; set; } = null!;

    public virtual Pbo Pbo { get; set; } = null!;
}
