using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.Enums;

namespace HMS.Shared.Entities
{
    [Table("Patients")]
    public class Patient : User
    {
        [Required]
        public BloodType BloodType { get; set; }

        [Required]
        public string EmergencyContact { get; set; }

        [Required]
        public string Allergies { get; set; } = ""; // csv for now..

        [Required]
        public float Weight { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
