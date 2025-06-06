using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Services.Implementations;
using HMS.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class NotificationViewModel
    {
        private readonly INotificationService _notification_service;

        public int userId { get; set; }
        public ObservableCollection<Notification> notificationsCollection { get; } = new();

        public NotificationViewModel(INotificationService notification_service, int user_id)
        {
            this._notification_service = notification_service;
            this.userId = user_id;
        }

        public async Task LoadAsync(int user_id)
        {
            IEnumerable<Notification> notifications = await this._notification_service.GetByUserIdAsync(user_id);

            this.notificationsCollection.Clear();

            foreach (Notification notification in notifications)
                this.notificationsCollection.Add(notification);
        }

        public async Task DeleteAsync(int notification_id)
        {
            await this._notification_service.DeteleAsync(notification_id);
            Notification notification = this.FindById(notification_id);
            if (notification != null)
                this.notificationsCollection.Remove(notification);
        }

        public Notification FindById(int id)
        {
            foreach (Notification notification in this.notificationsCollection)
            {
                if (notification.Id == id)
                    return notification;
            }
            return null;
        }
    }
}
