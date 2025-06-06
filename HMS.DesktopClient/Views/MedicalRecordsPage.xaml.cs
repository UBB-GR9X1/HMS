using HMS.DesktopClient.ViewModels;
using HMS.Shared.Proxies.Implementations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace HMS.DesktopClient.Views
{
    public sealed partial class MedicalRecordsPage : Page
    {
        public MedicalRecordHistoryViewModel? ViewModel { get; private set; }

        public MedicalRecordsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is int patientId)
            {
                var proxy = new MedicalRecordProxy(App.CurrentUser!.Token);
                ViewModel = new MedicalRecordHistoryViewModel(patientId, proxy);
                RecordsGrid.DataContext = ViewModel;
            }
        }
    }
}