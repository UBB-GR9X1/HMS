using HMS.DesktopClient.ViewModels;
using System.Net.Http; // Assuming HttpClient is needed
using Microsoft.UI.Xaml.Controls; // Corrected namespace
using Microsoft.UI.Xaml; // Corrected namespace
using HMS.Shared.Enums; // Add using directive for UserRole enum
using System;

namespace HMS.DesktopClient.Views
{
    public partial class ShiftView : Window
    {
        private ShiftViewModel viewModel; // Declare viewModel at class level

        // Modify the constructor to accept HttpClient, token, and user role
        public ShiftView(HttpClient httpClient, string token, int userRole)
        {
            InitializeComponent();

            // Create the ViewModel and pass dependencies
            viewModel = new ShiftViewModel(httpClient, token);

            // Set the user role
            viewModel.UserRole = userRole;

            // Set DataContext on the root element of the Window's content
            if (Content is FrameworkElement rootElement)
            {
                rootElement.DataContext = viewModel;
            }

            // Load shifts directly after setting DataContext
            _ = viewModel.LoadShiftsAsync();

            // NoAccessMessage visibility handled based on UserRole within the viewmodel or XAML binding
        }
    }
} 