using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notification_repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notification_repository">The notification repository used for data access.</param>
        public NotificationService(INotificationRepository notification_repository)
        {
            this._notification_repository = notification_repository;
        }

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the list of notifications for the given user.
        /// </returns>
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int user_id)
        {
            return await this._notification_repository.GetByUserIdAsync(user_id);
        }

        /// <summary>
        /// Deletes a single notification if it belongs to the specified user.
        /// </summary>
        /// <param name="notification_id">The unique identifier of the notification.</param>
        /// <param name="user_id">The unique identifier of the user attempting the deletion.</param>
        /// <returns>
        /// A task representing the asynchronous delete operation.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no notification with the specified ID exists.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the notification does not belong to the given user.
        /// </exception>
        public async Task DeteleAsync(int notification_id)
        {
            NotificationDto _notification = await this._notification_repository.GetByIdAsync(notification_id);

            if (_notification == null)
                throw new KeyNotFoundException(
                    $"Notification with ID {notification_id} not found.");

            await this._notification_repository.DeleteAsync(notification_id);
        }
    }
}
