using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HMS.Shared.Entities;

namespace HMS.DesktopClient.ViewModels
{
    public class AdminProfileViewModel : INotifyPropertyChanged
    {
        private readonly UserWithTokenDto _user;  // holds token and user info
        private AdminDto _admin;                  // holds admin-specific + user info (except token)
        private readonly AdminService _adminService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AdminProfileViewModel(UserWithTokenDto user, AdminDto admin)
        {
            _user = user;
            _admin = admin;
            var proxy = new AdminProxy(_user.Token);
            _adminService = new AdminService(proxy);
        }

        public string Name
        {
            get => _admin.Name ?? "";
            set
            {
                if (_admin.Name != value)
                {
                    _admin.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string PhoneNumber
        {
            get => _admin.PhoneNumber ?? "";
            set
            {
                if (_admin.PhoneNumber != value)
                {
                    _admin.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Email
        {
            get => _admin.Email ?? "";
            set
            {
                if (_admin.Email != value)
                {
                    _admin.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string CNP
        {
            get => _admin.CNP ?? "";
            set
            {
                if (_admin.CNP != value)
                {
                    _admin.CNP = value;
                    OnPropertyChanged(nameof(CNP));
                }
            }
        }

        public int Id => _admin.Id;
        public string Role => _admin.Role.ToString();
        public string CreatedAt => _admin.CreatedAt.ToString("yyyy-MM-dd HH:mm");


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

        public async Task LoadAdminAsync(int adminId)
        {
            var admin = await _adminService.GetAdminByIdAsync(adminId);
            if (admin == null)
                throw new Exception("Admin not found");
            _admin = admin;
            // Raise property changed for all properties that depend on _admin
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(CNP));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Role));
            OnPropertyChanged(nameof(CreatedAt));
        }
        private void OnPropertyChanged(string propertyName) =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
