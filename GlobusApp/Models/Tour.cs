using System;
using System.Collections.Generic;

namespace GlobusApp.Models;

public partial class Tour
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CountryId { get; set; }

    public int Duration { get; set; }

    public DateOnly StartDate { get; set; }

    public int Price { get; set; }

    public int BusId { get; set; }

    public int SeatsNotTaken { get; set; }

    public string ImageName { get; set; } = null!;
    public string ImageFullPath => !string.IsNullOrEmpty(ImageName)
    ? $"Images/{ImageName}" : "Images/icon.png";

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual BusType Bus { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;


    public bool IsLowSeats => Bus != null &&
    Bus.Capacity > 0 &&
    SeatsNotTaken <= Bus.Capacity * 0.1;

    // Тур скоро начнётся (менее 7 дней)
    public bool IsStartingSoon
    {
        get
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var daysToStart = StartDate.DayNumber - today.DayNumber;
            return daysToStart >= 0 && daysToStart <= 7;
        }
    }
}
