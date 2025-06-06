using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Equipment DTO operations.
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// Gets all equipments.
        /// </summary>
        /// <returns>List of Equipment DTOs.</returns>
        Task<List<EquipmentDto>> GetAllAsync();

        /// <summary>
        /// Gets equipment by id.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        /// <returns>Equipment DTO or null.</returns>
        Task<EquipmentDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds new equipment.
        /// </summary>
        /// <param name="equipment">Equipment DTO to add.</param>
        Task AddAsync(EquipmentDto equipment);

        /// <summary>
        /// Updates existing equipment.
        /// </summary>
        /// <param name="equipment">Equipment DTO to update.</param>
        Task UpdateAsync(EquipmentDto equipment);

        /// <summary>
        /// Deletes equipment by id.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Checks if equipment exists.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        /// <returns>True if exists.</returns>
        Task<bool> ExistsAsync(int id);
    }

}
