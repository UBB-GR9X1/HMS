using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.DTOs;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Appointment DTOs in the data store.
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Gets all appointments asynchronously.
        /// </summary>
        /// <returns>A collection of all appointments.</returns>
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        /// <summary>
        /// Gets an appointment by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the appointment.</param>
        /// <returns>The appointment if found; otherwise null.</returns>
        Task<AppointmentDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new appointment asynchronously.
        /// </summary>
        /// <param name="appointment">The appointment to add.</param>
        /// <returns>The added appointment.</returns>
        Task<AppointmentDto> AddAsync(AppointmentDto appointment);

        /// <summary>
        /// Updates an existing appointment asynchronously.
        /// </summary>
        /// <param name="appointment">The appointment with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(AppointmentDto appointment);

        /// <summary>
        /// Deletes an appointment by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the appointment to delete.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
