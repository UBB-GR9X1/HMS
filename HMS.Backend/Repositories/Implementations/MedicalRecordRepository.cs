using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing MedicalRecord entities.
    /// </summary>
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly MyDbContext _context;

        public MedicalRecordRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(m => m.Procedure)
                    .ThenInclude(p => p.Department)
                .AsNoTracking()  // This helps prevent circular reference tracking
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(m => m.Procedure)
                    .ThenInclude(p => p.Department)
                .AsNoTracking()  // This helps prevent circular reference tracking
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc />
        public async Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            
            // Reload the record with its relationships
            return await GetByIdAsync(medicalRecord.Id);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(MedicalRecord medicalRecord)
        {
            var existingRecord = await _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.Doctor)
                .Include(m => m.Procedure)
                .FirstOrDefaultAsync(m => m.Id == medicalRecord.Id);

            if (existingRecord == null) return false;

            // Update only the scalar properties
            existingRecord.PatientId = medicalRecord.PatientId;
            existingRecord.DoctorId = medicalRecord.DoctorId;
            existingRecord.ProcedureId = medicalRecord.ProcedureId;
            existingRecord.Diagnosis = medicalRecord.Diagnosis;
            existingRecord.CreatedAt = medicalRecord.CreatedAt;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.MedicalRecords.FindAsync(id);
            if (record == null) return false;

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
