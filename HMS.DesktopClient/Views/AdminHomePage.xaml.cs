using HMS.DesktopClient.ViewModels;
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
