using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels
{
    public class EquipmentViewModel : INotifyPropertyChanged
    {
        private readonly UserWithTokenDto _user;
        private EquipmentDto _equipment;
        private readonly EquipmentService _equipmentService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public EquipmentViewModel(UserWithTokenDto user, EquipmentDto equipment)
        {
            _user = user;
            _equipment = equipment;

            var proxy = new EquipmentProxy(_user.Token);
            _equipmentService = new EquipmentService(proxy);
        }

        public int Id
        {
            get => _equipment.Id;
            set
            {
                if (_equipment.Id != value)
                {
                    _equipment.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get => _equipment.Name ?? "";
            set
            {
                if (_equipment.Name != value)
                {
                    _equipment.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Specification
        {
            get => _equipment.Specification ?? "";
            set
            {
                if (_equipment.Specification != value)
                {
                    _equipment.Specification = value;
                    OnPropertyChanged(nameof(Specification));
                }
            }
        }

        public string Type
        {
            get => _equipment.Type ?? "";
            set
            {
                if (_equipment.Type != value)
                {
                    _equipment.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public int Stock
        {
            get => _equipment.Stock;
            set
            {
                if (_equipment.Stock != value)
                {
                    _equipment.Stock = value;
                    OnPropertyChanged(nameof(Stock));
                }
            }
        }

        // Token from _user
        public string Token => _user.Token;

        // Update equipment info
        public async Task<bool> UpdateEquipmentAsync()
        {
            await _equipmentService.UpdateAsync(_equipment);
            return true;
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 