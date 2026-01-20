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

namespace GlobusApp
{
    /// <summary>
    /// Логика взаимодействия для ApplicationsEditWindow.xaml
    /// </summary>
    public partial class ApplicationEditWindow : Window
    {
        private Models.Application _application;

        private List<User> _users;
        private List<Tour> _tours;
        private List<ApplicationStatus> _statuses;
        public ApplicationEditWindow(Models.Application application)
        {
            InitializeComponent();
            _application = application;

            LoadData();
            FillControls();
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
            StatusComboBox.ItemsSource = _statuses;
        }

        private void FillControls()
        {
            // Используем SelectedValue для корректного отображения текущего выбора
            UserComboBox.SelectedValue = _application.UserId;
            TourComboBox.SelectedValue = _application.TourId;
            StatusComboBox.SelectedValue = _application.StatusId;

            NumberOfPeopleTextBox.Text = _application.NumberOfPeople.ToString();
            CommentTextBox.Text = _application.Comment;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new GlobusTdbContext())
            {
                var appToUpdate = context.Applications
                    .Include(a => a.User)
                    .Include(a => a.Tour)
                    .Include(a => a.Status)
                    .FirstOrDefault(a => a.Id == _application.Id);


                if (appToUpdate != null)
                {
                    // Получаем существующие объекты из контекста по Id
                    var selectedUser = context.Users.Find((UserComboBox.SelectedItem as User)?.Id);
                    var selectedTour = context.Tours.Find((TourComboBox.SelectedItem as Tour)?.Id);
                    var selectedStatus = context.ApplicationStatuses.Find((StatusComboBox.SelectedItem as ApplicationStatus)?.Id);

                    if (selectedUser != null) appToUpdate.User = selectedUser;
                    if (selectedTour != null) appToUpdate.Tour = selectedTour;
                    if (selectedStatus != null) appToUpdate.Status = selectedStatus;

                    // Обновляем только количество человек
                    if (int.TryParse(NumberOfPeopleTextBox.Text, out int count))
                    {
                        appToUpdate.NumberOfPeople = count;
                    }
                    if (int.TryParse(TotalCostTextBlock.Text, out int price))
                    {
                        appToUpdate.TotalCost = price;
                    }

                    // Сохраняем изменения
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Заявка обновлена!");
            this.Close();
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
        if (TourComboBox.SelectedItem is Tour selectedTour && int.TryParse(NumberOfPeopleTextBox.Text, out int count))
        {
            decimal total = selectedTour.Price * count;
            TotalCostTextBlock.Text = total.ToString();
        }
        else
        {
            TotalCostTextBlock.Text = "0";
        }
    }
    }
}
