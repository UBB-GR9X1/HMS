using HMS.Shared.DTOs;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class PatientProfileViewModel : INotifyPropertyChanged
    {
        private readonly UserWithTokenDto _user;    // holds token and user info
        private PatientDto _patient;                 // holds patient-specific + user info (except token)
        private readonly PatientService _patientService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public PatientProfileViewModel(UserWithTokenDto user, PatientDto patient)
        {
            _user = user;
            _patient = patient;

            var proxy = new PatientProxy(_user.Token);
            _patientService = new PatientService(proxy);
        }

        // User fields come from _patient (inherited user fields)
        public string Name
        {
            get => _patient.Name ?? "";
            set
            {
                if (_patient.Name != value)
                {
                    _patient.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PhoneNumber
        {
            get => _patient.PhoneNumber ?? "";
            set
            {
                if (_patient.PhoneNumber != value)
                {
                    _patient.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string CNP
        {
            get => _patient.CNP ?? "";
            set
            {
                if (_patient.CNP != value)
                {
                    _patient.CNP = value;
                    OnPropertyChanged(nameof(CNP));
                }
            }
        }

        // Patient-only fields
        public string BloodType
        {
            get => _patient.BloodType ?? "";
            set
            {
                if (_patient.BloodType != value)
                {
                    _patient.BloodType = value;
                    OnPropertyChanged(nameof(BloodType));
                }
            }
        }

        public string EmergencyContact
        {
            get => _patient.EmergencyContact ?? "";
            set
            {
                if (_patient.EmergencyContact != value)
                {
                    _patient.EmergencyContact = value;
                    OnPropertyChanged(nameof(EmergencyContact));
                }
            }
        }

        public string Allergies
        {
            get => _patient.Allergies ?? "";
            set
            {
                if (_patient.Allergies != value)
                {
                    _patient.Allergies = value;
                    OnPropertyChanged(nameof(Allergies));
                }
            }
        }

        public float Weight
        {
            get => _patient.Weight;
            set
            {
                if (_patient.Weight != value)
                {
                    _patient.Weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        public float Height
        {
            get => _patient.Height;
            set
            {
                if (_patient.Height != value)
                {
                    _patient.Height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public DateTime BirthDate
        {
            get => _patient.BirthDate;
            set
            {
                if (_patient.BirthDate != value)
                {
                    _patient.BirthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public string Address
        {
            get => _patient.Address ?? "";
            set
            {
                if (_patient.Address != value)
                {
                    _patient.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public int Id => _patient.Id;
        public string Email => _patient.Email ?? "";
        public string Role => _patient.Role.ToString();
        public string CreatedAt => _patient.CreatedAt.ToString("yyyy-MM-dd HH:mm");

        public string Token => _user.Token;

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }


        public async Task<bool> UpdatePatientAsync()
        {
            try
            {
                return await _patientService.UpdatePatientAsync(_patient);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }
        }


        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
