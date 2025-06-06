using HMS.DesktopClient.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Views
{
    public sealed partial class AdminProfilePage : Page
    {
        // ViewModel for the Admin Profile Page can be added here if needed  
        public AdminProfileViewModel ViewModel { get; set; }  
        // Constructor  
        public AdminProfilePage()
        {
            this.InitializeComponent();
            ViewModel = new AdminProfileViewModel(App.CurrentUser, App.CurrentAdmin);
            this.DataContext = ViewModel;
            _ = LoadAdminDataAsync();
        }

        private async Task LoadAdminDataAsync()
        {
            try
            {
                await ViewModel.LoadAdminAsync(App.CurrentUser.Id);
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to load admin data: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        private async void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Update Profile",
                Content = "Profile update functionality is not implemented yet.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
