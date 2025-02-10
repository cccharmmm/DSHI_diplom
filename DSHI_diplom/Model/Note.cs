using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Note
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ComposerId { get; set; }

    public int? ClassId { get; set; }

    public int? MusicalformId { get; set; }

    public string? Description { get; set; }

    public DateOnly? DateOfCreate { get; set; }

    public int? FileId { get; set; }

    public int? InstrumentId { get; set; }

    public virtual AudioFile? AudioFile { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<CollectionOfNotesWithNote> CollectionOfNotesWithNotes { get; set; } = new List<CollectionOfNotesWithNote>();

    public virtual Composer? Composer { get; set; }

    public virtual ApplicationFile? File { get; set; }

    public virtual Instrument? Instrument { get; set; }

    public virtual MusicalForm? Musicalform { get; set; }
}
