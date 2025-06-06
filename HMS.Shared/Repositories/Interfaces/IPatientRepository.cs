using HMS.Shared.DTOs.Patient;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Patient entities in the data store.
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Gets all patients asynchronously.
        /// </summary>
        /// <returns>A collection of all patients.</returns>
        Task<IEnumerable<PatientDto>> GetAllAsync();

        /// <summary>
        /// Gets a patient by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>The patient if found, otherwise null.</returns>
        Task<PatientDto> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new patient asynchronously.
        /// </summary>
        /// <param name="patient">The patient to add.</param>
        /// <returns>The added patient.</returns>
        Task<PatientDto> AddAsync(PatientDto patient);

        /// <summary>
        /// Updates an existing patient asynchronously.
        /// </summary>
        /// <param name="patient">The patient with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(PatientUpdateDto patient, int id);

        /// <summary>
        /// Deletes a patient by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the patient to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
