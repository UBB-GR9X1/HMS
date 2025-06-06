using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Schedule DTOs in the data store.
    /// </summary>
    public interface IScheduleRepository
    {
        /// <summary>
        /// Gets all schedules asynchronously.
        /// </summary>
        /// <returns>A collection of all schedule DTOs.</returns>
        Task<IEnumerable<ScheduleDto>> GetAllAsync();

        /// <summary>
        /// Gets a schedule by composite keys asynchronously.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>The schedule DTO if found, otherwise null.</returns>
        Task<ScheduleDto?> GetByIdsAsync(int doctorId, int shiftId);

        /// <summary>
        /// Adds a new schedule asynchronously.
        /// </summary>
        /// <param name="schedule">The schedule DTO to add.</param>
        /// <returns>The added schedule DTO.</returns>
        Task<ScheduleDto> AddAsync(ScheduleDto schedule);

        /// <summary>
        /// Updates an existing schedule asynchronously.
        /// </summary>
        /// <param name="schedule">The schedule DTO with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(ScheduleDto schedule);

        /// <summary>
        /// Deletes a schedule asynchronously.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int doctorId, int shiftId);
    }
}
