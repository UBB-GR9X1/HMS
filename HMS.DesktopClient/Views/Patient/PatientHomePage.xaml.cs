using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Services.Implementations;
using HMS.Shared.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Net.Http;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientHomePage : Window
    {
        public PatientHomePage()
        {
            this.InitializeComponent();
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

        private void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            var patientId = App.CurrentUser!.Id;
            MainFrame.Navigate(typeof(MedicalRecordsPage), patientId);
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

        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PatientProfilePage));
        }

        private async void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = null;
        }

        private async void Notification_Click(object sender, RoutedEventArgs e)
        {
            INotificationRepository repo = new NotificationProxy(new HttpClient(), App.CurrentUser.Token);
            INotificationService service = new NotificationService(repo);
            NotificationViewModel viewModel = new NotificationViewModel(service, App.CurrentUser.Id);
            var notificationPage = new NotificationPage(viewModel);
            notificationPage.Activate();
        }
    }
}
