using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class TheoreticalMaterial
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? SubjectId { get; set; }

    public int? ClassId { get; set; }

    public int? AuthorId { get; set; }

    public int? FileId { get; set; }

    public string? Description { get; set; }

    public DateOnly? DateOfCreate { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<CollectionOfThMlWithThM> CollectionOfThMlWithThMs { get; set; } = new List<CollectionOfThMlWithThM>();

    public virtual ApplicationFile? File { get; set; }

    public virtual Subject? Subject { get; set; }
}
