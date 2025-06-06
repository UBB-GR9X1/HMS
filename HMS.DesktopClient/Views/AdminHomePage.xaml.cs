using HMS.DesktopClient.ViewModels;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class AdminHomePage : Window
    {
        //private ViewModels.AppointmentPage appointmentPage;
        public AdminHomePage()
        {
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

        private void appointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            var appointmentsPage = new AppointmentPage(App.CurrentUser);
            appointmentsPage.Activate();
        }
    }
}
