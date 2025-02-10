using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Question
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int TestId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Test Test { get; set; } = null!;
}
