using System;
using System.Collections.Generic;

namespace WpfApp1.MVVM.Model;

public partial class Spell
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Damage { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Monster> Monster { get; set; } = new List<Monster>();
}
