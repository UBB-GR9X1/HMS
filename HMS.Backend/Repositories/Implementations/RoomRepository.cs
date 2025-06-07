using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Repositories
{
    /// <inheritdoc/>
    public class RoomRepository : IRoomRepository
    {
        private readonly MyDbContext _context;

        public RoomRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Department)
                .Include(r => r.Equipments)
                .Include(r => r.Appointments)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Department)
                .Include(r => r.Equipments)
                .Include(r => r.Appointments)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<Room> AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }
    }
}
