using HMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.WebClient.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "CNP is required")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "CNP must be exactly 13 digits")]
        [Display(Name = "CNP")]
        public string CNP { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = "";

        [Required(ErrorMessage = "Years of experience is required")]
        [Display(Name = "Years of Experience")]
        [Range(0, 100, ErrorMessage = "Years of experience must be between 0 and 100")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "License number is required")]
        [Display(Name = "License Number")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters")]
        public string LicenseNumber { get; set; } = "";

        public List<int> ScheduleIds { get; set; } = new();
        public List<int> ReviewIds { get; set; } = new();
        public List<int> AppointmentIds { get; set; } = new();

        [Display(Name = "Number of Schedules")]
        public int SchedulesCount => ScheduleIds.Count;

        [Display(Name = "Number of Reviews")]
        public int ReviewsCount => ReviewIds.Count;

        [Display(Name = "Number of Appointments")]
        public int AppointmentsCount => AppointmentIds.Count;
    }
} 