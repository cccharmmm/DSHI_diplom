using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class CollectionOfThMlWithThM
{
    public int Id { get; set; }

    public int CollectionId { get; set; }

    public int ThmId { get; set; }

    public virtual CollectionOfTheoreticalMaterial Collection { get; set; } = null!;

    public virtual TheoreticalMaterial Thm { get; set; } = null!;
}
