using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for managing Department DTOs.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Retrieves all departments asynchronously.
        /// </summary>
        /// <returns>A collection of all departments.</returns>
        Task<IEnumerable<DepartmentDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a department by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the department.</param>
        /// <returns>The department if found; otherwise, null.</returns>
        Task<DepartmentDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new department asynchronously.
        /// </summary>
        /// <param name="department">The department to add.</param>
        /// <returns>The added department with assigned ID.</returns>
        Task<DepartmentDto> AddAsync(Department department);

        /// <summary>
        /// Updates an existing department asynchronously.
        /// </summary>
        /// <param name="department">The department with updated information.</param>
        /// <returns>True if update succeeded, otherwise false.</returns>
        Task<bool> UpdateAsync(Department department);

        /// <summary>
        /// Deletes a department by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the department to delete.</param>
        /// <returns>True if deletion succeeded, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
