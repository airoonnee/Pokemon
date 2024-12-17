using System;
using System.Collections.Generic;

namespace WpfApp1.MVVM.Model;

public partial class Monster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Health { get; set; }

    public int InitialHealth { get; set; } // Nouvelle propriété

    public string? ImageUrl { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<Spell> Spell { get; set; } = new List<Spell>();
}
