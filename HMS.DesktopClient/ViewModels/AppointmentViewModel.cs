using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Enums;
using HMS.Shared.Repositories.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HMS.Shared.Proxies.Implementations;
using System.Net.Http;
using HMS.Shared.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HMS.DesktopClient.ViewModels
{
    public class AppointmentPage : INotifyPropertyChanged
    {
        private readonly UserWithTokenDto _user;
        private readonly AppointmentService _appointmentService;
        public ObservableCollection<AppointmentDto> Appointments { get; } = new ObservableCollection<AppointmentDto>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public AppointmentPage(UserWithTokenDto user)
        {
            _user = user;
            var proxy = new AppointmentProxy(_user.Token);
            // Configure HttpClient to ignore object references
            proxy.HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            proxy.HttpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            // The following line attempts to send serializer options via a header, which the server may not support.
            // The primary deserialization is still governed by the options in AppointmentProxy.
            // If ReferenceHandler.Ignore is not available, the original error with ReferenceHandler.Preserve will likely persist
            // unless backend serialization is adjusted or a different deserializer is used.
            // proxy.HttpClient.DefaultRequestHeaders.Add("X-JsonSerializerOptions", JsonSerializer.Serialize(new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Ignore }));
            _appointmentService = new AppointmentService(proxy);
        }

        // Token from _user
        public string Token => _user.Token;

        public async Task LoadAppointmentsAsync()
        {
            Appointments.Clear();
            try
            {
                var allAppointments = await _appointmentService.GetAllAsync();

                if (_user.Role == UserRole.Admin)
                {
                    foreach (var appointment in allAppointments)
                    {
                        Appointments.Add(appointment);
                    }
                }
                else if (_user.Role == UserRole.Doctor)
                {
                    int currentDoctorId = _user.Id;
                    foreach (var appointment in allAppointments)
                    {
                        if (appointment.DoctorId == currentDoctorId)
                        {
                            Appointments.Add(appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Implement proper error handling/logging
                Console.WriteLine($"Error loading appointments: {ex.Message}");
                throw; // Re-throw to handle in the View
            }
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            try
            {
                await _appointmentService.DeleteAsync(id);
                await LoadAppointmentsAsync(); // Reload the list after deletion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting appointment: {ex.Message}");
                throw; // Re-throw to handle in the View
            }
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentDto appointment)
        {
            try
            {
                await _appointmentService.UpdateAsync(appointment);
                await LoadAppointmentsAsync(); // Reload the list after update
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating appointment: {ex.Message}");
                throw; // Re-throw to handle in the View
            }
        }

        public async Task<bool> AddAppointmentAsync(AppointmentDto appointment)
        {
            try
            {
                await _appointmentService.AddAsync(appointment);
                await LoadAppointmentsAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw;
            }
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
