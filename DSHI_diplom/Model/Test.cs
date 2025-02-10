using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Test
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ClassId { get; set; }

    public int? SubjectId { get; set; }

    public string? Description { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual Subject? Subject { get; set; }

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
