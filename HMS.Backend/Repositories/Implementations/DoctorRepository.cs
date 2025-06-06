using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Doctor entities.
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MyDbContext _context;

        public DoctorRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Schedules)
                .Include(d => d.Reviews)
                .Include(d => d.Appointments)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Schedules)
                .Include(d => d.Reviews)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <inheritdoc />
        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Doctor doctor)
        {
            var existingDoctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == doctor.Id);
                
            if (existingDoctor == null) return false;

            // Update scalar properties
            _context.Entry(existingDoctor).CurrentValues.SetValues(doctor);
            
            // Update Department relationship
            if (doctor.DepartmentId != existingDoctor.DepartmentId)
            {
                var department = await _context.Departments.FindAsync(doctor.DepartmentId);
                if (department == null) return false;
                existingDoctor.Department = department;
                existingDoctor.DepartmentId = doctor.DepartmentId;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Doctor>> GetByDepartmentIdAsync(int departmentId)
        {
            var doctors = await _context.Doctors
                .Include(d => d.Department)
                .Include(d => d.Schedules)
                .Include(d => d.Reviews)
                .Include(d => d.Appointments)
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();

            return doctors;
        }
    }
}
