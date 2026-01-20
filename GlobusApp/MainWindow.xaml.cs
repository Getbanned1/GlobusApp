using GlobusApp.Data;
using GlobusApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlobusApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Role userRole;
    public MainWindow(Role role)
    {
        InitializeComponent();
        userRole = role;
        MainFrame.Navigate(new TourPage(this, role));
        CheckUserRole();
    }
    private void CheckUserRole()
    {
        if (userRole.Name == "Гость")
        {
            NavigationMenu.Visibility = Visibility.Hidden;

        }
        if (userRole.Name == "Администратор" )
        {
            NavigationMenu.Visibility = Visibility.Visible;
        }
        if (userRole.Name == "Менеджер")
        {
            NavigationMenu.Visibility = Visibility.Visible;
        }
    }


    private void AddTourButton_Click(object sender, RoutedEventArgs e)
    {

    }


    private void ToursNavClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TourPage(this, userRole));

    }

    private void Applicationsnav_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Applications(this));
    }
}