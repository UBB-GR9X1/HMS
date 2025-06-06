using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Services;

namespace HMS.DesktopClient.ViewModels.Patient
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private PatientService PatientService;
        private AppointmentService AppointmentService;
        private ObservableCollection<PatientDto> _patients;

        public PatientViewModel(PatientService patientService, AppointmentService appointmentService)
        {
            PatientService = patientService;
            AppointmentService = appointmentService;
        }

        public async Task InitializeAsync()
        {
            await LoadPatientsAsync();
        }

        public ObservableCollection<PatientDto> Patients
        {
            get => _patients;
            private set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }


        private async Task LoadPatientsAsync()
        {
            try
            {
                Patients = new ObservableCollection<PatientDto>();
                var appointments = await AppointmentService.GetAppointmentsForDoctor(App.CurrentDoctor.Id);
                var patientIds = appointments.Select(a => a.PatientId).Distinct().ToList();
                if (patientIds.Count == 0)
                {
                    Patients = new ObservableCollection<PatientDto>();
                    return;
                }
                Debug.WriteLine($"Found {patientIds.Count} unique patient IDs from appointments for doctor with ID {App.CurrentDoctor.Id}.");
                foreach (var patient in patientIds)
                {
                    var patientDto = await PatientService.GetPatientByIdAsync(patient);
                    if (patientDto != null)
                    {
                        Patients.Add(patientDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patients: {ex.Message}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
