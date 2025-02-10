using System;
using System.Collections.Generic;

namespace DSHI_diplom.Model;

public partial class User
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<CollectionOfNote> CollectionOfNotes { get; set; } = new List<CollectionOfNote>();

    public virtual ICollection<CollectionOfTheoreticalMaterial> CollectionOfTheoreticalMaterials { get; set; } = new List<CollectionOfTheoreticalMaterial>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
