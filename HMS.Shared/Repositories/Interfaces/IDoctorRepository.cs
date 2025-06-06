using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Doctor DTOs in the data store.
    /// </summary>
    public interface IDoctorRepository
    {
        /// <summary>
        /// Gets all doctors asynchronously.
        /// </summary>
        /// <returns>A collection of all doctors.</returns>
        Task<IEnumerable<DoctorDto>> GetAllAsync();

        /// <summary>
        /// Gets a doctor by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the doctor.</param>
        /// <returns>The doctor if found, otherwise null.</returns>
        Task<DoctorDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new doctor asynchronously.
        /// </summary>
        /// <param name="doctor">The doctor to add.</param>
        /// <returns>The added doctor.</returns>
        Task<DoctorDto> AddAsync(DoctorDto doctor);

        /// <summary>
        /// Updates an existing doctor asynchronously.
        /// </summary>
        /// <param name="doctor">The doctor with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(DoctorDto doctor);

        /// <summary>
        /// Deletes a doctor by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the doctor to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
