using System;

namespace Hospital.ViewModels.MedicalRecord
{
    public class MedicalRecordListItemViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorDepartment { get; set; }
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string ProcedureDepartment { get; set; }
        public string Diagnosis { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
