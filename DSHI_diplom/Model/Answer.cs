using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Answer
{
    public int Id { get; set; }

    public string Variant { get; set; } = null!;

    public bool Rightt { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
