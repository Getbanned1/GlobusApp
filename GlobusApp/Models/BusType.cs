using System;
using System.Collections.Generic;

namespace GlobusApp.Models;

public partial class BusType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
