using GlobusApp.Data;
using GlobusApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlobusApp
{
    /// <summary>
    /// Логика взаимодействия для TourPage.xaml
    /// </summary>
    public partial class TourPage : Page
    {
        private MainWindow _mainWindow;

        public TourPage(MainWindow mainWindow, Role role)
        {
            InitializeComponent(); 
            LoadTours();
            _mainWindow = mainWindow;
            SearchMenu.Visibility = role.Name == "Гость" ? Visibility.Hidden : Visibility.Visible;
         }
        private void LoadTours()
        {
            using (var context = new GlobusTdbContext())
            {

                ToursCard.ItemsSource = context.Tours
                    .Include(t => t.Country)      // Загрузка связанной страны
                    .Include(t => t.Bus)          // Загрузка связанного автобуса
                    .Include(t => t.Applications)

                    .ToList();

            }
        }
        private void UpdateTours()
        {
            using (var context = new GlobusTdbContext())
            {

                var query  = context.Tours
                    .Include(t => t.Country)      // Загрузка связанной страны
                    .Include(t => t.Bus)          // Загрузка связанного автобуса
                    .Include(t => t.Applications).AsQueryable();
                if (!string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    string searchText = SearchBar.Text.ToLower();
                    query = query.Where(t => t.Name.ToLower().Contains(searchText) || t.Id.ToString().Contains(searchText));
                }
                ToursCard.ItemsSource = query.ToList();
            }
        }
        //private Tour _selectedTour;
        //public Tour SelectedTour
        //{
        //    get => _selectedTour;
        //    set
        //    {
        //        _selectedTour = value;
        //        OnPropertyChanged();

        //        if (value != null)
        //            OpenTour(value);
        //    }
        //}

        private void OpenTour(Tour tour)
        {
            // навигация / открытие деталей
        }



        private void Tours_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }
    }
}
