using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.Proxies.Implementations;

namespace HMS.Shared.Services
{
    public class AppointmentService
    {
        private readonly AppointmentProxy _appointmentProxy;
        public AppointmentService(AppointmentProxy appointmentProxy)
        {
            _appointmentProxy = appointmentProxy;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return (List<AppointmentDto>)await _appointmentProxy.GetAllAsync();
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            return await _appointmentProxy.GetByIdAsync(id);
        }

        public async Task AddAsync(AppointmentDto appointment)
        {
            await _appointmentProxy.AddAsync(appointment);
        }

        public async Task UpdateAsync(AppointmentDto appointment)
        {
            await _appointmentProxy.UpdateAsync(appointment);
        }

        public async Task DeleteAsync(int id)
        {
            await _appointmentProxy.DeleteAsync(id);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsForDoctor(int doctorId)
        {
            if (doctorId <= 0)
                throw new ArgumentException("Invalid doctor ID", nameof(doctorId));
            IEnumerable<AppointmentDto> appointmentDtos = await _appointmentProxy.GetAllAsync();
            List<AppointmentDto> res = appointmentDtos.Where(a => a.DoctorId == doctorId).ToList();
            Debug.WriteLine($"Found {res.Count} appointments for doctor with ID {doctorId}.");
            return res;
        }


    }
}
