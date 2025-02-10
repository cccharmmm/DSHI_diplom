using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class AudioFile
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public int? NotesId { get; set; }

    public virtual Note? Notes { get; set; }
}
