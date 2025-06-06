using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using HMS.DesktopClient.ViewModels;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotificationPage : Window
    {
        public NotificationViewModel _notification_view_model { get; }

        public NotificationPage(NotificationViewModel view_model)
        {
            this.InitializeComponent();
            this._notification_view_model = view_model;
            LoadNotifications();
        }

        private async void LoadNotifications()
        {
            await this._notification_view_model.LoadAsync(_notification_view_model.userId);
        }

        private async void deleteButtonClick(object sender, RoutedEventArgs routed_event)
        {
            Button button = sender as Button;
            if (button != null)
            {

                Notification notification = button.DataContext as Notification;
                if (notification != null)
                {
                    await this._notification_view_model.DeleteAsync(notification.Id);
                }
            }
        }
    }
}
