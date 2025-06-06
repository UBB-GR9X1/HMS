using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Proxies.Implementations;

namespace HMS.DesktopClient.ViewModels
{
    public class MedicalRecordCreationFormViewModel : INotifyPropertyChanged
    {
        // Dependencies
        private readonly DoctorProxy _doctorProxy;
        private readonly ProcedureProxy _procedureProxy;
        private readonly DepartmentProxy _departmentProxy;
        private readonly PatientProxy _patientProxy;

        // Lists
        public ObservableCollection<DoctorDto> DoctorsList { get; } = new();
        public ObservableCollection<DepartmentDto> DepartmentsList { get; } = new();
        public ObservableCollection<PatientDto> PatientsList { get; } = new();
        public ObservableCollection<ProcedureDto> ProceduresList { get; } = new();

        // Selections
        private DoctorDto? _selectedDoctor;
        public DoctorDto? SelectedDoctor
        {
            get => _selectedDoctor;
            set { _selectedDoctor = value; OnPropertyChanged(nameof(SelectedDoctor)); }
        }

        private PatientDto? _selectedPatient;
        public PatientDto? SelectedPatient
        {
            get => _selectedPatient;
            set { _selectedPatient = value; OnPropertyChanged(nameof(SelectedPatient)); }
        }

        private ProcedureDto? _selectedProcedure;
        public ProcedureDto? SelectedProcedure
        {
            get => _selectedProcedure;
            set { _selectedProcedure = value; OnPropertyChanged(nameof(SelectedProcedure)); }
        }

        private DepartmentDto? _selectedDepartment;
        public DepartmentDto? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
                _ = LoadDoctorsAndProceduresAsync(0);
_ = LoadDoctorsAndProceduresAsync(DepartmentsList.IndexOf(_selectedDepartment) + 1);
            }
        }

        // Inputs
        private DateTimeOffset _appointmentDateOffset = DateTimeOffset.Now;
        public DateTimeOffset AppointmentDateOffset
        {
            get => _appointmentDateOffset;
            set { _appointmentDateOffset = value; OnPropertyChanged(nameof(AppointmentDateOffset)); }
        }

        private string _appointmentTime;
        public string AppointmentTime
        {
            get => _appointmentTime;
            set { _appointmentTime = value; OnPropertyChanged(nameof(AppointmentTime)); }
        }

        private string _conclusion;
        public string Conclusion
        {
            get => _conclusion;
            set { _conclusion = value; OnPropertyChanged(nameof(Conclusion)); }
        }

        public ObservableCollection<string> DocumentPaths { get; } = new();

        public void AddDocument(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                DocumentPaths.Add(path);
        }

        public DateTime AppointmentDate
        {
            get
            {
                if (TimeSpan.TryParse(AppointmentTime, out var time))
                    return AppointmentDateOffset.Date + time;
                return AppointmentDateOffset.Date;
            }
        }

        public async Task InitializeAsync()
        {
            var departments = await _departmentProxy.GetAllAsync();
            DepartmentsList.Clear();
            foreach (var dept in departments)
                DepartmentsList.Add(dept);

            var patients = await _patientProxy.GetAllAsync();
            PatientsList.Clear();
            foreach (var patient in patients)
                PatientsList.Add(patient);
        }

        // Load doctors and procedures by department
        private async Task LoadDoctorsAndProceduresAsync(int departmentId)
        {
            DoctorsList.Clear();
            ProceduresList.Clear();

            if (departmentId == 0) return;

            var doctors = await _doctorProxy.GetAllAsync();
            foreach (var doctor in doctors.Where(d => d.DepartmentId == departmentId))
                DoctorsList.Add(doctor);

            var procedures = await _procedureProxy.GetAllAsync();
            foreach (var proc in procedures.Where(p => p.DepartmentId == departmentId))
                ProceduresList.Add(proc);

            SelectedDoctor = null;
            SelectedProcedure = null;

            Debug.WriteLine($"Loaded {DoctorsList.Count} doctors and {ProceduresList.Count} procedures.");
        }

        // Constructor
        public MedicalRecordCreationFormViewModel(
            DoctorProxy doctorProxy,
            ProcedureProxy procedureProxy,
            DepartmentProxy departmentProxy,
            PatientProxy patientProxy)
        {
            _doctorProxy = doctorProxy;
            _procedureProxy = procedureProxy;
            _departmentProxy = departmentProxy;
            _patientProxy = patientProxy;
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}