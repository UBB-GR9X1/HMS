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

namespace HMS.DesktopClient.Views.Doctor
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorHomePage : Window
    {
        public DoctorHomePage()
        {
            this.InitializeComponent();
        }

       private void Appointments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Appointments",
                Content = "Appointments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void MedicalRecords_Click(object sender, RoutedEventArgs e)
        {
            var doctorId = App.CurrentUser!.Id;
            MainFrame.Navigate(typeof(MedicalRecordsPage), doctorId);
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PatientsDisplayPage));
        }

        private void Equipments_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Equipments",
                Content = "Equipments button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Rooms",
                Content = "Rooms button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Schedule",
                Content = "Schedule button clicked.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DoctorProfilePage));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = null;
        }

    }
}
