using HMS.DesktopClient.Views.Patient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using HMS.DesktopClient.Views;
using System.Net.Http;
using HMS.DesktopClient.APIClients;
using Microsoft.Extensions.DependencyInjection;
using HMS.Shared.Enums;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminHomePage : Window
    {
        private readonly UserApiClient _userApiClient;

        public AdminHomePage()
        {
            _userApiClient = App.Services.GetRequiredService<UserApiClient>();
            this.InitializeComponent();
        }

        private async void Procedures_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Procedures",
                Content = "Procedures button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Rooms_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Rooms",
                Content = "Rooms button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Equipments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Equipments",
                Content = "Equipments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Doctors_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Doctors",
                Content = "Doctors button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PatientsDisplayPage));
        }

        private async void Appointments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Appointments",
                Content = "Appointments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Schedules_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Schedules",
                Content = "Schedules button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Shifts_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Shifts",
                Content = "Shifts button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Medical Records",
                Content = "Medical Records button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(AdminHomePage));
        }

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
           MainFrame.Navigate(typeof(AdminProfilePage));
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            var adminDashboard = new LogsPage();
            adminDashboard.Activate();
        }

        private void ShiftsButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = App.Services.GetRequiredService<HttpClient>();

            if (App.CurrentUser != null)
            {
                var authToken = App.CurrentUser.Token;
                var userRole = (int)App.CurrentUser.Role;

                var shiftWindow = new ShiftView(httpClient, authToken, userRole);
                shiftWindow.Activate();
            }
            else
            {
                Console.WriteLine("Error: User not logged in.");
                // TODO: Implement proper handling for not logged in user.
            }
        }
    }
}
