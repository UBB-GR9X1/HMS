using HMS.DesktopClient.Utils;
using HMS.DesktopClient.ViewModels;
using HMS.Shared.Enums;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Net.Http;

namespace HMS.DesktopClient.Views
{
    public sealed partial class LogsPage : Window
    {
        private readonly LoggerViewModel _loggerViewModel;

        public LogsPage()
        {
            InitializeComponent();

            // Only show admin features for admin users
            if (App.CurrentUser?.Role != UserRole.Admin)
            {
                ShowErrorDialog("Access Denied", "You don't have permission to access this page.");
                return;
            }

            // Initialize the LoggerViewModel with LoggerService
            var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5203/api/") };
            var loggerProxy = new LoggerProxy(httpClient, App.CurrentUser.Token);
            var loggerService = new LoggerService(loggerProxy);
            _loggerViewModel = new LoggerViewModel(loggerService);

            // Configure UI bindings for logs
            ConfigureLoggerUI();

            // Load logs by default on entry
            _loggerViewModel.loadAllLogsCommand.Execute(null);

            // Make sure to update the UI
            System.Diagnostics.Debug.WriteLine($"Loaded {_loggerViewModel.logs.Count} logs into the view model");
        }

        private async void ShowErrorDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = Content.XamlRoot
            };

            await dialog.ShowAsync();
            this.Close();
        }

        private void ConfigureLoggerUI()
        {
            // Set the item source for ListView
            LogListView.ItemsSource = _loggerViewModel.logs;

            // Set up ComboBox for action types
            ActionTypeComboBox.ItemsSource = _loggerViewModel.actionTypes;

            // Bind TextBox for user ID filtering
            UserIdTextBox.SetBinding(TextBox.TextProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("user_id_input"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
                UpdateSourceTrigger = Microsoft.UI.Xaml.Data.UpdateSourceTrigger.PropertyChanged
            });

            // Bind ComboBox for action type filtering - using SelectedItem instead of SelectedValue
            ActionTypeComboBox.SetBinding(ComboBox.SelectedItemProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("selected_action_type"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
                UpdateSourceTrigger = Microsoft.UI.Xaml.Data.UpdateSourceTrigger.PropertyChanged
            });

            // Bind DatePicker for timestamp filtering
            TimestampDatePicker.SetBinding(DatePicker.DateProperty, new Microsoft.UI.Xaml.Data.Binding
            {
                Path = new PropertyPath("selected_timestamp"),
                Source = _loggerViewModel,
                Mode = Microsoft.UI.Xaml.Data.BindingMode.TwoWay,
            });

            // Bind the buttons directly to commands in the ViewModel
            LoadAllLogsButton.Command = _loggerViewModel.loadAllLogsCommand;
            LoadLogsByUserIdButton.Command = _loggerViewModel.filterLogsByUserIdCommand;
            LoadLogsByActionTypeButton.Command = _loggerViewModel.filterLogsByActionTypeCommand;
            LoadLogsBeforeTimestampButton.Command = _loggerViewModel.filterLogsByTimestampCommand;
            ApplyFiltersButton.Command = _loggerViewModel.applyAllFiltersCommand;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}