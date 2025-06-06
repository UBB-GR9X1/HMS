using HMS.DesktopClient.ViewModels;
using HMS.DesktopClient.ViewModels.Doctor;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace HMS.DesktopClient.Views.Doctor
{
    public sealed partial class DoctorProfilePage : Page
    {
        public DoctorProfileViewModel ViewModel { get; set; }

        public DoctorProfilePage()
        {
            this.InitializeComponent();
            ViewModel = new DoctorProfileViewModel(App.CurrentUser, App.CurrentDoctor);
            this.DataContext = ViewModel;
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            bool success = await ViewModel.UpdateDoctorAsync();

            ContentDialog dialog = new ContentDialog
            {
                Title = success ? "Success" : "Error",
                Content = success ? "Profile updated successfully." : "Failed to update profile.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}
