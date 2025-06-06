using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDto doctor)
        {
            return await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            return await _doctorRepository.DeleteAsync(id);
        }

        public async Task<DoctorDto> AddDoctorAsync(DoctorDto doctor)
        {
            return await _doctorRepository.AddAsync(doctor);
        }
    }
}
