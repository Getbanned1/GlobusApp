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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace GlobusApp
{
    /// <summary>
    /// Логика взаимодействия для ApplicationsEditWindow.xaml
    /// </summary>
    public partial class ApplicationCreateWindow : Window
    {
        public event Action? ApplicationSaved;
        private Models.Application _application;

        private List<User> _users;
        private List<Tour> _tours;
        private List<ApplicationStatus> _statuses;
        public ApplicationCreateWindow()
        {
            InitializeComponent();

            LoadData();
            DataTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void LoadData()
        {
            using (var context = new GlobusTdbContext())
            {
                _users = context.Users.ToList();
                _tours = context.Tours.Include(t => t.Country).ToList();
                _statuses = context.ApplicationStatuses.ToList();
            }

            UserComboBox.ItemsSource = _users;
            TourComboBox.ItemsSource = _tours;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {


            // Валидация
            if (UserComboBox.SelectedItem == null ||
                TourComboBox.SelectedItem == null ||
                !int.TryParse(NumberOfPeopleTextBox.Text, out int peopleCount) ||
                !DateOnly.TryParse(DataTextBox.Text, out DateOnly date))
            {
                MessageBox.Show("Проверьте корректность введённых данных");
                return;
            }


            var user = (User)UserComboBox.SelectedItem;
            var tour = (Tour)TourComboBox.SelectedItem;

            using (var context = new GlobusTdbContext())
            {
                var application = new Models.Application
                {
                    UserId = user.Id,
                    TourId = tour.Id,
                    Date = date,
                    NumberOfPeople = peopleCount,
                    TotalCost = tour.Price * peopleCount,
                    StatusId = 2, // "Новая"
                    Comment = CommentTextBox.Text
                };

                if (tour.SeatsNotTaken < peopleCount)
                {
                    MessageBox.Show($"Недостаточно мест. Свободно: {tour.SeatsNotTaken}");
                    return;
                }
                tour.SeatsNotTaken -= peopleCount;

                context.Applications.Add(application);
                context.SaveChanges();
                ApplicationSaved?.Invoke();
            }
            ;


            MessageBox.Show("Заявка добавлена");
            Close();
        }

        private void TourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecalculateTotalCost();
        }

        private void NumberOfPeopleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecalculateTotalCost();
        }
        private void RecalculateTotalCost()
        {
            if (TourComboBox.SelectedItem is Tour tour &&
                int.TryParse(NumberOfPeopleTextBox.Text, out int count))
            {
                TotalCostTextBlock.Text = (tour.Price * count).ToString();
            }
            else
            {
                TotalCostTextBlock.Text = "0";
            }
        }

    }
}
