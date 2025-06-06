using HMS.Shared.DTOs;
using HMS.Shared.Entities;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Room DTOs.
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Retrieves all Room DTOs.
        /// </summary>
        /// <returns>List of Room DTOs</returns>
        Task<List<RoomDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a Room DTO by ID.
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>Room DTO if found; otherwise null</returns>
        Task<RoomDto?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new Room DTO.
        /// </summary>
        /// <param name="room">Room DTO to add</param>
        Task AddAsync(RoomDto room);

        /// <summary>
        /// Updates an existing Room DTO.
        /// </summary>
        /// <param name="room">Room DTO with updated data</param>
        Task UpdateAsync(RoomDto room);

        /// <summary>
        /// Deletes a Room DTO by ID.
        /// </summary>
        /// <param name="id">Room ID</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Checks if a Room exists by ID.
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>True if exists; otherwise false</returns>
        Task<bool> ExistsAsync(int id);
    }
}
