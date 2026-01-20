using GlobusApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AuthService authService;
        public LoginWindow()
        {
            InitializeComponent();
            authService = new AuthService();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Role role = authService.TryAuth(LoginTextBox.Text, PasswordTextBox.Password);
                if (role != null)
                {
                    var mainWindow = new MainWindow(role);
                    mainWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось войти в аккаунт: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            Role role = new();
            role.Name = "Гость";
            var mainWindow = new MainWindow(role);
            mainWindow.Show();
            this.Close();
        }
    }
}
