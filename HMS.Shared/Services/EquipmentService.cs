using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class EquipmentService
    {
        private readonly EquipmentProxy _equipmentProxy;

        public EquipmentService(EquipmentProxy equipmentProxy)
        {
            _equipmentProxy = equipmentProxy;
        }

        public async Task<List<EquipmentDto>> GetAllAsync()
        {
            return await _equipmentProxy.GetAllAsync();
        }

        public async Task<EquipmentDto?> GetByIdAsync(int id)
        {
            return await _equipmentProxy.GetByIdAsync(id);
        }

        public async Task AddAsync(EquipmentDto equipment)
        {
            await _equipmentProxy.AddAsync(equipment);
        }

        public async Task UpdateAsync(EquipmentDto equipment)
        {
            await _equipmentProxy.UpdateAsync(equipment);
        }

        public async Task DeleteAsync(int id)
        {
            await _equipmentProxy.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _equipmentProxy.ExistsAsync(id);
        }
    }
} 