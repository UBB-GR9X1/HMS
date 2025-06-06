using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Procedure repository handling CRUD operations with ProcedureDto.
    /// </summary>
    public interface IProcedureRepository
    {
        /// <summary>
        /// Gets all procedures.
        /// </summary>
        Task<IEnumerable<ProcedureDto>> GetAllAsync();

        /// <summary>
        /// Gets a procedure by ID.
        /// </summary>
        /// <param name="id">The ID of the procedure to retrieve.</param>
        Task<ProcedureDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new procedure.
        /// </summary>
        /// <param name="procedure">The procedure DTO to add.</param>
        /// <returns>The added procedure DTO.</returns>
        Task<ProcedureDto> AddAsync(ProcedureDto procedure);

        /// <summary>
        /// Updates an existing procedure.
        /// </summary>
        /// <param name="procedure">The procedure DTO with updated information.</param>
        /// <returns>True if update succeeded, otherwise false.</returns>
        Task<bool> UpdateAsync(ProcedureDto procedure);

        /// <summary>
        /// Deletes a procedure by ID.
        /// </summary>
        /// <param name="id">The ID of the procedure to delete.</param>
        /// <returns>True if deletion succeeded, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
