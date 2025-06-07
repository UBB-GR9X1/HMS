using HMS.Shared.Entities;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Room entities.
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Retrieves all Room entities.
        /// </summary>
        /// <returns>List of Room entities</returns>
        Task<List<Room>> GetAllAsync();

        /// <summary>
        /// Retrieves a Room entity by ID.
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>Room entity if found; otherwise null</returns>
        Task<Room?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new Room entity.
        /// </summary>
        /// <param name="room">Room entity to add</param>
        Task<Room> AddAsync(Room room);

        /// <summary>
        /// Updates an existing Room entity.
        /// </summary>
        /// <param name="room">Room entity with updated data</param>
        Task UpdateAsync(Room room);

        /// <summary>
        /// Deletes a Room entity by ID.
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
