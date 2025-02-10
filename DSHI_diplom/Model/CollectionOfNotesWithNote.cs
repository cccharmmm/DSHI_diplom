using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class CollectionOfNotesWithNote
{
    public int Id { get; set; }

    public int CollectionId { get; set; }

    public int NotesId { get; set; }

    public virtual CollectionOfNote Collection { get; set; } = null!;

    public virtual Note Notes { get; set; } = null!;
}
