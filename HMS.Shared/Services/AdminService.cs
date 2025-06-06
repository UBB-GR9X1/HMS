using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }



        public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
        {
            return await _adminRepository.GetAllAsync();
        }

        public async Task<AdminDto?> GetAdminByIdAsync(int id)
        {
            return await _adminRepository.GetByIdAsync(id);
        }

        public async Task<AdminDto?> GetAdminByEmailAsync(string email)
        {
            return await _adminRepository.GetByEmailAsync(email);
        }

        public async Task<AdminDto> AddAdminAsync(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin), "Admin cannot be null");
            return await _adminRepository.AddAsync(admin);
        }

        public async Task<bool> UpdateAdminAsync(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin), "Admin cannot be null");
            return await _adminRepository.UpdateAsync(admin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid admin ID", nameof(id));
            return await _adminRepository.DeleteAsync(id);
        }
    }
}