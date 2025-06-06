using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class AppointmentService
    {
        private readonly AppointmentProxy _appointmentProxy;

        public AppointmentService(AppointmentProxy appointmentProxy)
        {
            _appointmentProxy = appointmentProxy;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            return await _appointmentProxy.GetAllAsync();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            return await _appointmentProxy.GetByIdAsync(id);
        }

        public async Task AddAsync(AppointmentDto equipment)
        {
            await _appointmentProxy.AddAsync(equipment);
        }

        public async Task UpdateAsync(AppointmentDto equipment)
        {
            await _appointmentProxy.UpdateAsync(equipment);
        }


        public async Task DeleteAsync(int id)
        {
            await _appointmentProxy.DeleteAsync(id);
        }

        //public async Task<bool> ExistsAsync(int id)
        //{
        //    return await _appointmentProxy.ExistsAsync(id);
        //}

    }
}