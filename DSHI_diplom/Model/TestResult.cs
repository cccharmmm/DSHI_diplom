using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class TestResult
{
    public int Id { get; set; }

    public int TestId { get; set; }

    public int UserId { get; set; }

    public int CorrectAnswers { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
