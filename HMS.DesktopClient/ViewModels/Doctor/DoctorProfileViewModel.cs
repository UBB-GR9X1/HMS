using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Doctor
{
    public class DoctorProfileViewModel
    {
        private readonly UserWithTokenDto _user;
        private DoctorDto _doctor;
        private readonly DoctorService _doctorService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DoctorProfileViewModel(UserWithTokenDto user, DoctorDto doctor)
        {
            _user = user;
            _doctor = doctor;

            var proxy = new DoctorProxy(_user.Token);
            _doctorService = new DoctorService(proxy);
        }

        // Common user fields
        public string Name
        {
            get => _doctor.Name;
            set
            {
                if (_doctor.Name != value)
                {
                    _doctor.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PhoneNumber
        {
            get => _doctor.PhoneNumber;
            set
            {
                if (_doctor.PhoneNumber != value)
                {
                    _doctor.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string CNP
        {
            get => _doctor.CNP;
            set
            {
                if (_doctor.CNP != value)
                {
                    _doctor.CNP = value;
                    OnPropertyChanged(nameof(CNP));
                }
            }
        }

        // Doctor-specific fields
        public int DepartmentId
        {
            get => _doctor.DepartmentId;
            set
            {
                if (_doctor.DepartmentId != value)
                {
                    _doctor.DepartmentId = value;
                    OnPropertyChanged(nameof(DepartmentId));
                }
            }
        }

        public string DepartmentName
        {
            get => _doctor.DepartmentName;
            set
            {
                if (_doctor.DepartmentName != value)
                {
                    _doctor.DepartmentName = value;
                    OnPropertyChanged(nameof(DepartmentName));
                }
            }
        }

        public int YearsOfExperience
        {
            get => _doctor.YearsOfExperience;
            set
            {
                if (_doctor.YearsOfExperience != value)
                {
                    _doctor.YearsOfExperience = value;
                    OnPropertyChanged(nameof(YearsOfExperience));
                }
            }
        }

        public string LicenseNumber
        {
            get => _doctor.LicenseNumber;
            set
            {
                if (_doctor.LicenseNumber != value)
                {
                    _doctor.LicenseNumber = value;
                    OnPropertyChanged(nameof(LicenseNumber));
                }
            }
        }

        // Read-only
        public int Id => _doctor.Id;
        public string Email => _doctor.Email;
        public string Role => _doctor.Role.ToString();
        public string CreatedAt => _doctor.CreatedAt.ToString("yyyy-MM-dd HH:mm");

        public string Token => _user.Token;

        // Update
        public async Task<bool> UpdateDoctorAsync()
        {
            return await _doctorService.UpdateDoctorAsync(_doctor);
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
