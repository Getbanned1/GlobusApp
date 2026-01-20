using System;
using System.Collections.Generic;

namespace GlobusApp.Models;

public partial class Application
{
    public int Id { get; set; }

    public int TourId { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public int StatusId { get; set; }

    public int NumberOfPeople { get; set; }

    public int TotalCost { get; set; }

    public string? Comment { get; set; }

    public virtual ApplicationStatus Status { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
