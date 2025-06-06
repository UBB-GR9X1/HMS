using HMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        
        Task DeteleAsync(int notificationId);
    }
}
