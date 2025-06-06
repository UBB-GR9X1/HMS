using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using System.Net.Http;
using System;
using Microsoft.UI.Xaml;
using HMS.Shared.Enums;

namespace HMS.DesktopClient.Views
{
    public sealed partial class SchedulePage : Window
    {
        public ScheduleViewModel ViewModel { get; }

        public SchedulePage()
        {
            this.InitializeComponent();

            // Create HttpClient and ScheduleProxy directly
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5203/api/") };
            // Ensure App.CurrentUser and its Token are available before creating the proxy
            if (App.CurrentUser == null || string.IsNullOrEmpty(App.CurrentUser.Token))
            {
                 // Handle the case where user is not logged in or token is missing
                 // Perhaps show an error or navigate back to login page
                 Console.WriteLine("User not logged in or token missing.");
                 // Example: this.Close(); // Close the schedule window if user is not authenticated
                 // return;
            }

            var scheduleProxy = new ScheduleProxy(httpClient, App.CurrentUser.Token);

            // Initialize the ViewModel with the created proxy
            ViewModel = new ScheduleViewModel(scheduleProxy);

            // Load schedules when the window is activated
            this.Activated += SchedulePage_Activated;
        }

        private async void SchedulePage_Activated(object sender, WindowActivatedEventArgs args)
        {
             if (args.WindowActivationState == WindowActivationState.CodeActivated ||
                 args.WindowActivationState == WindowActivationState.PointerActivated)
            {
                await ViewModel.LoadSchedulesAsync();
            }
        }
    }
}
