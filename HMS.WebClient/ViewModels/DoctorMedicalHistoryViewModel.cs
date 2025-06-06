using HMS.Shared.DTOs;
using System.Collections.Generic;

namespace HMS.WebClient.ViewModels
{
    public class DoctorMedicalHistoryViewModel
    {
        public string DoctorName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public List<MedicalRecordDto> MedicalRecords { get; set; } = new();
        public List<PatientSummaryViewModel> RecentPatients { get; set; } = new();
        public List<AppointmentDto> UpcomingAppointments { get; set; } = new();
    }

    public class PatientSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime LastVisit { get; set; }
        public int VisitCount { get; set; }
    }
} 