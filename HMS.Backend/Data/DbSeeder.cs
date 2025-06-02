using HMS.Shared.Entities;
using HMS.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using HMS.Backend.Utils;

namespace HMS.Backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(MyDbContext context)
        {
            // Check if database is already seeded
            if (await context.Users.AnyAsync())
                return;

            // Create departments
            var departments = new List<Department>
            {
                new Department { Name = "Cardiology" },
                new Department { Name = "Neurology" },
                new Department { Name = "Pediatrics" },
                new Department { Name = "General Surgery" },
                new Department { Name = "Orthopedics" },
                new Department { Name = "Ophthalmology" },
                new Department { Name = "ENT" },
                new Department { Name = "Dermatology" }
            };
            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();

            // Create admin
            var admin = new Admin
            {
                Email = "admin@hms.com",
                Password = PasswordHasher.HashPassword("admin123"),
                Name = "Administrator",
                CNP = "1234567890123",
                PhoneNumber = "0712345678",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };
            await context.Admins.AddAsync(admin);

            // Create doctors
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Email = "ion.popescu@hms.com",
                    Password = PasswordHasher.HashPassword("doctor123"),
                    Name = "Dr. Ion Popescu",
                    CNP = "1780512345678",
                    PhoneNumber = "0723456789",
                    Role = UserRole.Doctor,
                    CreatedAt = DateTime.UtcNow,
                    DepartmentId = departments[0].Id, // Cardiology
                    YearsOfExperience = 15,
                    LicenseNumber = "MD12345"
                },
                new Doctor
                {
                    Email = "maria.ionescu@hms.com",
                    Password = PasswordHasher.HashPassword("doctor123"),
                    Name = "Dr. Maria Ionescu",
                    CNP = "2810623456789",
                    PhoneNumber = "0734567890",
                    Role = UserRole.Doctor,
                    CreatedAt = DateTime.UtcNow,
                    DepartmentId = departments[1].Id, // Neurology
                    YearsOfExperience = 12,
                    LicenseNumber = "MD12346"
                },
                new Doctor
                {
                    Email = "andrei.popa@hms.com",
                    Password = PasswordHasher.HashPassword("doctor123"),
                    Name = "Dr. Andrei Popa",
                    CNP = "1750734567890",
                    PhoneNumber = "0745678901",
                    Role = UserRole.Doctor,
                    CreatedAt = DateTime.UtcNow,
                    DepartmentId = departments[2].Id, // Pediatrics
                    YearsOfExperience = 8,
                    LicenseNumber = "MD12347"
                }
            };
            await context.Doctors.AddRangeAsync(doctors);

            // Create patients
            var patients = new List<Patient>
            {
                new Patient
                {
                    Email = "george.dumitrescu@email.com",
                    Password = PasswordHasher.HashPassword("patient123"),
                    Name = "George Dumitrescu",
                    CNP = "1890845678901",
                    PhoneNumber = "0756789012",
                    Role = UserRole.Patient,
                    CreatedAt = DateTime.UtcNow,
                    BloodType = BloodType.A_Positive,
                    EmergencyContact = "0767890123",
                    Allergies = "Polen, Penicilină",
                    Weight = 75.5f,
                    Height = 178.0f,
                    BirthDate = new DateOnly(1989, 8, 4),
                    Address = "Str. Primăverii nr. 10, București"
                },
                new Patient
                {
                    Email = "elena.stan@email.com",
                    Password = PasswordHasher.HashPassword("patient123"),
                    Name = "Elena Stan",
                    CNP = "2920956789012",
                    PhoneNumber = "0767890123",
                    Role = UserRole.Patient,
                    CreatedAt = DateTime.UtcNow,
                    BloodType = BloodType.B_Negative,
                    EmergencyContact = "0778901234",
                    Allergies = "Lactate",
                    Weight = 62.0f,
                    Height = 165.0f,
                    BirthDate = new DateOnly(1992, 9, 5),
                    Address = "Str. Victoriei nr. 25, București"
                },
                new Patient
                {
                    Email = "mihai.constantin@email.com",
                    Password = PasswordHasher.HashPassword("patient123"),
                    Name = "Mihai Constantin",
                    CNP = "1850667890123",
                    PhoneNumber = "0778901234",
                    Role = UserRole.Patient,
                    CreatedAt = DateTime.UtcNow,
                    BloodType = BloodType.O_Positive,
                    EmergencyContact = "0789012345",
                    Allergies = "",
                    Weight = 85.0f,
                    Height = 182.0f,
                    BirthDate = new DateOnly(1985, 6, 6),
                    Address = "Str. Libertății nr. 15, București"
                }
            };
            await context.Patients.AddRangeAsync(patients);

            // Create equipment
            var equipment = new List<Equipment>
            {
                new Equipment
                {
                    Name = "Electrocardiograph",
                    Specification = "Model: ECG-2000, Manufacturer: Philips",
                    Type = "Diagnostic",
                    Stock = 5
                },
                new Equipment
                {
                    Name = "Ultrasound Machine",
                    Specification = "Model: Voluson E10, Manufacturer: GE Healthcare",
                    Type = "Imaging",
                    Stock = 3
                },
                new Equipment
                {
                    Name = "Surgical Kit",
                    Specification = "Complete set of sterile surgical instruments",
                    Type = "Surgery",
                    Stock = 10
                },
                new Equipment
                {
                    Name = "Vital Signs Monitor",
                    Specification = "Model: IntelliVue MX450, Manufacturer: Philips",
                    Type = "Monitoring",
                    Stock = 8
                }
            };
            await context.Equipments.AddRangeAsync(equipment);

            // Create rooms
            var rooms = new List<Room>
            {
                new Room
                {
                    DepartmentId = departments[0].Id, // Cardiology
                    Capacity = 2,
                    Equipments = new List<Equipment> { equipment[0], equipment[3] } // ECG and monitor
                },
                new Room
                {
                    DepartmentId = departments[1].Id, // Neurology
                    Capacity = 1,
                    Equipments = new List<Equipment> { equipment[1], equipment[3] } // Ultrasound and monitor
                },
                new Room
                {
                    DepartmentId = departments[3].Id, // Surgery
                    Capacity = 1,
                    Equipments = new List<Equipment> { equipment[2], equipment[3] } // Surgical kit and monitor
                }
            };
            await context.Rooms.AddRangeAsync(rooms);

            // Create procedures
            var procedures = new List<Procedure>
            {
                new Procedure
                {
                    Name = "Cardiac Consultation",
                    DepartmentId = departments[0].Id,
                    Duration = new TimeSpan(0, 30, 0) // 30 minutes
                },
                new Procedure
                {
                    Name = "ECG",
                    DepartmentId = departments[0].Id,
                    Duration = new TimeSpan(0, 15, 0) // 15 minutes
                },
                new Procedure
                {
                    Name = "Neurological Consultation",
                    DepartmentId = departments[1].Id,
                    Duration = new TimeSpan(0, 45, 0) // 45 minutes
                },
                new Procedure
                {
                    Name = "Ultrasound Examination",
                    DepartmentId = departments[1].Id,
                    Duration = new TimeSpan(0, 20, 0) // 20 minutes
                }
            };
            await context.Procedures.AddRangeAsync(procedures);

            // Create shifts
            var shifts = new List<Shift>
            {
                new Shift
                {
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    StartTime = new TimeOnly(8, 0), // 8:00
                    EndTime = new TimeOnly(16, 0)   // 16:00
                },
                new Shift
                {
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    StartTime = new TimeOnly(16, 0), // 16:00
                    EndTime = new TimeOnly(0, 0)     // 24:00
                },
                new Shift
                {
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    StartTime = new TimeOnly(8, 0),  // 8:00
                    EndTime = new TimeOnly(16, 0)    // 16:00
                }
            };
            await context.Shifts.AddRangeAsync(shifts);

            // Create schedules
            var schedules = new List<Schedule>
            {
                new Schedule
                {
                    DoctorId = doctors[0].Id,
                    ShiftId = shifts[0].Id
                },
                new Schedule
                {
                    DoctorId = doctors[1].Id,
                    ShiftId = shifts[1].Id
                },
                new Schedule
                {
                    DoctorId = doctors[2].Id,
                    ShiftId = shifts[2].Id
                }
            };
            await context.Schedules.AddRangeAsync(schedules);

            // Create appointments
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    ProcedureId = procedures[0].Id,
                    RoomId = rooms[0].Id,
                    DateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 0, 0)
                },
                new Appointment
                {
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    ProcedureId = procedures[2].Id,
                    RoomId = rooms[1].Id,
                    DateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14, 0, 0)
                }
            };
            await context.Appointments.AddRangeAsync(appointments);

            // Create medical records
            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord
                {
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    ProcedureId = procedures[0].Id,
                    Diagnosis = "Hypertension",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new MedicalRecord
                {
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    ProcedureId = procedures[2].Id,
                    Diagnosis = "Chronic Migraine",
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                }
            };
            await context.MedicalRecords.AddRangeAsync(medicalRecords);

            // Create reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    PatientId = patients[0].Id,
                    DoctorId = doctors[0].Id,
                    Value = 5
                },
                new Review
                {
                    PatientId = patients[1].Id,
                    DoctorId = doctors[1].Id,
                    Value = 4
                }
            };
            await context.Reviews.AddRangeAsync(reviews);

            await context.SaveChangesAsync();
        }
    }
} 