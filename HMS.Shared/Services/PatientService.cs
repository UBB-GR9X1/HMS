using HMS.Shared.DTOs.Patient;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        private static readonly HashSet<string> AllowedBloodTypes = new HashSet<string>
        {
            "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };

        private static bool IsOnlyLettersOrSpaces(string text)
        {
            return text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private static bool IsDigitsOnly(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> UpdatePatientAsync(PatientDto patient)
        {
            if (!IsOnlyLettersOrSpaces(patient.Name))
                throw new Exception("The name can contain only letters.");

            if (!IsDigitsOnly(patient.PhoneNumber) || patient.PhoneNumber.Length != 10)
                throw new Exception("The phone number is invalid. It need to have 10 digits.");

            if (!IsDigitsOnly(patient.CNP) || patient.CNP.Length != 13)
                throw new Exception("The CNP format is invalid. It need to have 13 digits.");

            if (!TryFormatBloodTypeForServer(patient.BloodType, out string formattedBloodType))
                throw new Exception("Invalid blood type. Must be one of: A+, A-, B+, B-, AB+, AB-, O+, or O-.");

            if (!IsDigitsOnly(patient.EmergencyContact) || patient.EmergencyContact.Length != 10)
                throw new Exception("The emercy contact isn't a valid phone number.");


            var updateDto = MapToUpdateDto(patient);
            updateDto.BloodType = formattedBloodType; // e.g., "A_Positive"

            return await _patientRepository.UpdateAsync(updateDto, patient.Id);
        }


        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            return await _patientRepository.DeleteAsync(id);
        }

        public async Task<PatientDto> AddPatientAsync(PatientDto patient)
        {
            if (!IsOnlyLettersOrSpaces(patient.Name))
                throw new Exception("The name can contain only letters.");

            if (!IsDigitsOnly(patient.PhoneNumber) || patient.PhoneNumber.Length != 10)
                throw new Exception("The phone number is invalid. It need to have 10 digits.");

            if (!IsDigitsOnly(patient.CNP) || patient.CNP.Length != 13)
                throw new Exception("The CNP format is invalid. It need to have 13 digits.");

            if (!IsValidBloodType(patient.BloodType))
                throw new Exception("Invalid blood type. Must be one of: A+, A-, B+, B-, AB+, AB-, O+, or O-.");

            if (!IsDigitsOnly(patient.EmergencyContact) || patient.EmergencyContact.Length != 10)
                throw new Exception("The emercy contact isn't a valid phone number.");

            return await _patientRepository.AddAsync(patient);
        }

        private static bool IsValidBloodType(string bloodType)
        {
            if (string.IsNullOrWhiteSpace(bloodType))
                return false;

            return AllowedBloodTypes.Contains(bloodType.Trim().ToUpper());
        }

        private PatientUpdateDto MapToUpdateDto(PatientDto patient)
        {
            return new PatientUpdateDto
            {
                Id = patient.Id,
                Email = patient.Email,
                Password = patient.Password,
                Name = patient.Name,
                CNP = patient.CNP,
                PhoneNumber = patient.PhoneNumber,
                Role = patient.Role.ToString(),
                BloodType = patient.BloodType ?? "",
                EmergencyContact = patient.EmergencyContact ?? "",
                Allergies = patient.Allergies ?? "",
                Weight = patient.Weight,
                Height = patient.Height,
                BirthDate = patient.BirthDate,
                Address = patient.Address ?? ""
            };
        }

        private static bool TryFormatBloodTypeForServer(string input, out string formatted)
        {
            formatted = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            string normalized = input.Trim().ToUpper();

            var valid = new Dictionary<string, string>
            {
                { "A+", "A_Positive" }, { "A-", "A_Negative" },
                { "B+", "B_Positive" }, { "B-", "B_Negative" },
                { "AB+", "AB_Positive" }, { "AB-", "AB_Negative" },
                { "O+", "O_Positive" }, { "O-", "O_Negative" },
                { "A_POSITIVE", "A_Positive" }, { "A_NEGATIVE", "A_Negative" },
                { "B_POSITIVE", "B_Positive" }, { "B_NEGATIVE", "B_Negative" },
                { "AB_POSITIVE", "AB_Positive" }, { "AB_NEGATIVE", "AB_Negative" },
                { "O_POSITIVE", "O_Positive" }, { "O_NEGATIVE", "O_Negative" }
            };

            return valid.TryGetValue(normalized, out formatted);
        }


    }
}
