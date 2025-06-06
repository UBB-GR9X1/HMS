using HMS.Shared.DTOs.Patient;
using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class LoginService
    {
        private readonly PatientProxy _patientProxy;
        private readonly DoctorProxy _doctorProxy;
        private readonly AdminProxy _adminProxy;

        public LoginService(PatientProxy patientProxy, DoctorProxy doctorProxy, AdminProxy adminProxy)
        {
            _patientProxy = patientProxy;
            _doctorProxy = doctorProxy;
            _adminProxy = adminProxy;
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            return await _patientProxy.GetByIdAsync(id);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            return await _doctorProxy.GetByIdAsync(id);
        }

        public async Task<AdminDto> GetAdminByIdAsync(int id)
        {
            return await _adminProxy.GetByIdAsync(id);
        }
    }
}
