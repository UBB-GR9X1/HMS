using HMS.Shared.DTOs.Patient;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        private static readonly Dictionary<string, string> BloodTypeMapping = new Dictionary<string, string>
        {
            { "A+", "A_Positive" },
            { "A-", "A_Negative" },
            { "B+", "B_Positive" },
            { "B-", "B_Negative" },
            { "AB+", "AB_Positive" },
            { "AB-", "AB_Negative" },
            { "O+", "O_Positive" },
            { "O-", "O_Negative" }
        };

        // Reverse mapping for display
        private static readonly Dictionary<string, string> ReverseBloodTypeMapping = new Dictionary<string, string>
        {
            { "A_Positive", "A+" },
            { "A_Negative", "A-" },
            { "B_Positive", "B+" },
            { "B_Negative", "B-" },
            { "AB_Positive", "AB+" },
            { "AB_Negative", "AB-" },
            { "O_Positive", "O+" },
            { "O_Negative", "O-" }
        };

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> UpdatePatientAsync(PatientUpdateDto patient)
        {
            // Convert the display blood type to the backend format
            if (!string.IsNullOrEmpty(patient.BloodType) && BloodTypeMapping.ContainsKey(patient.BloodType))
            {
                patient.BloodType = BloodTypeMapping[patient.BloodType];
            }
            else if (!string.IsNullOrEmpty(patient.BloodType))
            {
                throw new Exception($"Invalid blood type: {patient.BloodType}. Must be one of: A+, A-, B+, B-, AB+, AB-, O+, or O-.");
            }

            return await _patientRepository.UpdateAsync(patient, patient.Id);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            // Convert the backend blood type format to display format
            if (!string.IsNullOrEmpty(patient.BloodType))
            {
                if (patient.BloodType.Contains("_"))
                {
                    if (ReverseBloodTypeMapping.ContainsKey(patient.BloodType))
                    {
                        patient.BloodType = ReverseBloodTypeMapping[patient.BloodType];
                    }
                }
            }

            return patient;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            // Convert blood types for display
            foreach (var patient in patients)
            {
                if (!string.IsNullOrEmpty(patient.BloodType) && ReverseBloodTypeMapping.ContainsKey(patient.BloodType))
                {
                    patient.BloodType = ReverseBloodTypeMapping[patient.BloodType];
                }
            }

            return patients;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }
    }
}