using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class CollectionOfNote
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int UserId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CollectionOfNotesWithNote> CollectionOfNotesWithNotes { get; set; } = new List<CollectionOfNotesWithNote>();

    public virtual User User { get; set; } = null!;
}
