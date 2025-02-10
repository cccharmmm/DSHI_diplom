using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class ApplicationFile
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public virtual Note? Note { get; set; }

    public virtual ICollection<TheoreticalMaterial> TheoreticalMaterials { get; set; } = new List<TheoreticalMaterial>();
}
