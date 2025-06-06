using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for managing User DTOs.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A collection of all users.</returns>
        Task<IEnumerable<UserDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<UserDto?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their email address asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<UserDto?> GetByEmailAsync(string email);

        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="user">The user DTO to add.</param>
        /// <returns>The added user DTO.</returns>
        Task<UserDto> AddAsync(UserDto user);

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user DTO with updated information.</param>
        /// <returns>True if update succeeded, otherwise false.</returns>
        Task<bool> UpdateAsync(UserDto user);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if deletion succeeded, otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
