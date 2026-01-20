using GlobusApp.Data;
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
    /// Логика взаимодействия для Applications.xaml
    /// </summary>
    public partial class Applications : Page
    {
        private MainWindow _mainWindow;
        public Applications(MainWindow mainWindow)
        {
            InitializeComponent();
            LoadApplications();
            _mainWindow = mainWindow;
            UpdateApplications();
            LoadStatuses();
        }
        private void LoadStatuses()
        {
            using (var context = new GlobusTdbContext())
            {
                // Получаем список всех статусов
                var statuses = context.ApplicationStatuses
                                      .Select(s => s.Status)
                                      .ToList();

                // Добавляем элемент "Все" для отмены фильтра
                statuses.Insert(0, "Все");

                SortBox.ItemsSource = statuses;

                // По умолчанию выбран "Все"
                SortBox.SelectedIndex = 0;
            }
            DataSortBox.ItemsSource = new List<string> { "По возрастанию", "По убыванию" };
            DataSortBox.SelectedIndex = 0; // По умолчанию сортировка по возрастанию
        }

        private void LoadApplications()
        {
            using (var context = new GlobusTdbContext())
            {

                ApplicationsView.ItemsSource = context.Applications
                .Include(a => a.User)
                .Include(a => a.Tour)
                    .ThenInclude(t => t.Country)
                .Include(a => a.Status)
                .ToList();
            }
        }
        public void UpdateApplications()
        {
            using (var context = new GlobusTdbContext())
            {
                // Базовый запрос с нужными Include
                var query = context.Applications
                    .Include(a => a.User)
                    .Include(a => a.Tour)
                        .ThenInclude(t => t.Country)
                    .Include(a => a.Status)
                    .AsQueryable();

                // Поиск по имени пользователя или стране
                if (!string.IsNullOrWhiteSpace(SearchBar.Text))
                {
                    string searchText = SearchBar.Text.ToLower();

                    query = query.Where(a =>
                        a.User.FullName.ToLower().Contains(searchText) ||
                        a.Tour.Country.CountryName.ToLower().Contains(searchText) ||
                        a.Id.ToString().Contains(searchText)
                    );
                }

                // Фильтрация по статусу
                if (SortBox.SelectedItem != null && SortBox.SelectedItem.ToString() != "Все")
                {
                    string selectedStatus = SortBox.SelectedItem.ToString()!;
                    query = query.Where(a => a.Status.Status == selectedStatus);
                }
                // Сортировка по дате
                if (DataSortBox.SelectedItem != null)
                {
                    if (DataSortBox.SelectedItem.ToString() == "По возрастанию")
                        query = query.OrderBy(a => a.Date);
                    else
                        query = query.OrderByDescending(a => a.Date);
                }

                // Применяем к ListView
                ApplicationsView.ItemsSource = query.ToList();
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateApplications();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateApplications();
        }

        private void DataSortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateApplications();
        }
        private void ApplicationsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationsView.SelectedItem is Models.Application selectedApplication)
            {
                var editWindow = new ApplicationEditWindow(selectedApplication);
                editWindow.ShowDialog();

                // Обновляем ListView после редактирования
                LoadApplications();
            }
        }

        private void AddClickButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ApplicationCreateWindow();

            window.ApplicationSaved += () =>
            {
                UpdateApplications(); // перезагружаем ListView
            };

            window.ShowDialog();
        }
    }
}
