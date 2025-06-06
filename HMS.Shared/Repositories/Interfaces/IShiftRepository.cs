using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Shift DTOs and their schedules.
    /// </summary>
    public interface IShiftRepository
    {
        /// <summary>
        /// Retrieves all shifts asynchronously.
        /// </summary>
        /// <returns>A list of all Shift DTOs.</returns>
        Task<IEnumerable<ShiftDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific shift by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The shift ID.</param>
        /// <returns>The Shift DTO if found; otherwise null.</returns>
        Task<ShiftDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new shift asynchronously.
        /// </summary>
        /// <param name="shift">The Shift DTO to add.</param>
        /// <returns>The added Shift DTO.</returns>
        Task<ShiftDto> AddAsync(ShiftDto shift);

        /// <summary>
        /// Updates an existing shift asynchronously.
        /// </summary>
        /// <param name="shift">The Shift DTO with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(ShiftDto shift);

        /// <summary>
        /// Deletes a shift by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The shift ID.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
