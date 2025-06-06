using System;
using HMS.DesktopClient.APIClients;
using HMS.DesktopClient.Views;
using HMS.Shared.DTOs;
using HMS.Shared.DTOs.Patient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public static UserWithTokenDto? CurrentUser { get; set; } = null;
        public static PatientDto? CurrentPatient { get; set; } = null;
        public static AdminDto? CurrentAdmin { get; set; } = null;
        public static DoctorDto? CurrentDoctor { get; set; } = null;  

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UserApiClient>();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            m_window = new LoginPage();
            m_window.Activate();
        }

        private Window? m_window;
    }
}
