using System;
using System.Collections.Generic;

namespace DiplomApplication.DataBase;

public partial class FeedBackRate
{
    public int FeedbackRateId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<FeedBack> FeedBacks { get; } = new List<FeedBack>();
}
