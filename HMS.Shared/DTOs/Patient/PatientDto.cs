using System;
using System.Collections.Generic;

namespace HMS.Shared.DTOs.Patient
{
    public class PatientDto : UserDto
    {
        public string BloodType { get; set; } = null!;

        public string EmergencyContact { get; set; } = null!;

        public string Allergies { get; set; } = "";

        public float Weight { get; set; }

        public float Height { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Address { get; set; } = null!;

        public List<int> ReviewIds { get; set; } = new List<int>();

        public List<int> AppointmentIds { get; set; } = new List<int>();

        public List<int> MedicalRecordIds { get; set; } = new List<int>();
    }
}
