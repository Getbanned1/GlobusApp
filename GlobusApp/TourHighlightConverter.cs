using GlobusApp.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GlobusApp
{


    public class TourHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Tour tour)
            {
                //if (tour.IsSpecialOffer)
                //    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD700"));

                if (tour.IsLowSeats)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB6C1"));
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
