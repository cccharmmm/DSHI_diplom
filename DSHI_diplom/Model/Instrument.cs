using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Instrument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
