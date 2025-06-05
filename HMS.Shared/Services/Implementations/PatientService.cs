using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private IPatientRepository _patient_proxy;

        public PatientService(IPatientRepository patient_proxy) {
            this._patient_proxy = patient_proxy;
        }

        public async Task<bool> AddPatient(Patient patient)
        {
            try
            {
                var addedPatient = await _patient_proxy.AddAsync(patient);
                if (addedPatient != null)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Failed to add patient.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while adding the patient.", ex);
            }
        }
    }
}
