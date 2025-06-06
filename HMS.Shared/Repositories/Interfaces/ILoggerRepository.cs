using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Log repository handling CRUD operations using LogDto.
    /// </summary>
    public interface ILoggerRepository
    {
        /// <summary>
        /// Gets all logs.
        /// </summary>
        /// <returns>A collection of all Log DTOs.</returns>
        Task<IEnumerable<LogDto>> GetAllAsync();

        /// <summary>
        /// Gets a log by ID.
        /// </summary>
        /// <param name="id">The ID of the log.</param>
        /// <returns>The log DTO with the specified ID or null if not found.</returns>
        Task<LogDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new log.
        /// </summary>
        /// <param name="log">The log DTO to add.</param>
        /// <returns>The added log DTO.</returns>
        Task<LogDto> AddAsync(LogDto log);

        /// <summary>
        /// Updates an existing log.
        /// </summary>
        /// <param name="log">The log DTO with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(LogDto log);

        /// <summary>
        /// Deletes a log by ID.
        /// </summary>
        /// <param name="id">The ID of the log to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
