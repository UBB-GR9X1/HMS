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
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Enums;
using HMS.Shared.Proxies.Implementations;
using System.Net.Http;
using HMS.Shared.Services;
using HMS.DesktopClient.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentPage : Window
    {
        private readonly ViewModels.AppointmentPage _viewModel;

        public AppointmentPage(UserWithTokenDto user)
        {
            this.InitializeComponent();
            _viewModel = new ViewModels.AppointmentPage(user);
            LoadAppointments();
        }

        private async void LoadAppointments()
        {
            try
            {
                await _viewModel.LoadAppointmentsAsync();
                AppointmentListView.ItemsSource = _viewModel.Appointments;
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to load appointments: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        private async void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                try
                {
                    await _viewModel.DeleteAppointmentAsync(id);
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Failed to delete appointment: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement add new appointment dialog or navigation
            var dialog = new ContentDialog
            {
                Title = "Add New",
                Content = "Add new appointment functionality not implemented yet.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            dialog.ShowAsync();
        }
    }
}
