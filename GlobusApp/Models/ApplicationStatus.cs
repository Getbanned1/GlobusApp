using System;
using System.Collections.Generic;

namespace GlobusApp.Models;

public partial class ApplicationStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
