using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TheoreticalMaterial> TheoreticalMaterials { get; set; } = new List<TheoreticalMaterial>();
}
