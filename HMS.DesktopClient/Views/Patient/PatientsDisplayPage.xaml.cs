using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HMS.DesktopClient.ViewModels.Patient;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views.Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientsDisplayPage : Page
    {
        public PatientViewModel ViewModel { get; set; }
        public PatientsDisplayPage()
        {
            this.InitializeComponent();
            ViewModel = new PatientViewModel(new PatientService(new PatientProxy(App.CurrentUser.Token)),
                new AppointmentService(new AppointmentProxy(App.CurrentUser.Token)));
            PatientsGrid.DataContext = ViewModel;
            _ = ViewModel.InitializeAsync();
        }
    }
}
