using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface defining data operations for Notification DTOs.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Retrieves all notifications asynchronously.
        /// </summary>
        /// <returns>A collection of all notification DTOs.</returns>
        Task<IEnumerable<NotificationDto?>> GetAllAsync();

        /// <summary>
        /// Retrieves a notification by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The notification's unique ID.</param>
        /// <returns>The matching notification DTO or null if not found.</returns>
        Task<NotificationDto?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all notifications for a specific user by their ID asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int id);

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="notification">Notification DTO to add.</param>
        /// <returns>The added notification DTO.</returns>
        Task<NotificationDto> AddAsync(NotificationDto notification);

        /// <summary>
        /// Updates an existing notification asynchronously.
        /// </summary>
        /// <param name="notification">Notification DTO with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(NotificationDto notification);

        /// <summary>
        /// Deletes a notification by id asynchronously.
        /// </summary>
        /// <param name="id">The unique ID of the notification to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
