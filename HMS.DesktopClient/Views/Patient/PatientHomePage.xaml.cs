using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

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
    }
}
