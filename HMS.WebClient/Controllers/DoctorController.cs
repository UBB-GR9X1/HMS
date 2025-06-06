using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.WebClient.Services;
using HMS.WebClient.ViewModels;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using HMS.Shared.Enums;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Doctor)]
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IProcedureRepository _procedureRepository;
        private readonly AuthService _authService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(
            DoctorService doctorService,
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IProcedureRepository procedureRepository,
            AuthService authService,
            ILogger<DoctorController> logger)
        {
            _doctorService = doctorService;
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _procedureRepository = procedureRepository;
            _authService = authService;
            _logger = logger;
        }

        public IActionResult Index() => RedirectToAction(nameof(Profile));

        public async Task<IActionResult> Profile()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                    return RedirectToAction("Login", "Account");

                var doctorViewModel = await _doctorService.GetDoctorByIdAsync(currentUser.Id);
                if (doctorViewModel == null)
                    return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");

                return View(doctorViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Profile action");
                ModelState.AddModelError("", "An error occurred while loading the profile.");
                return View(new DoctorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Id != id)
                return Forbid();

            var doctorViewModel = await _doctorService.GetDoctorByIdAsync(id);
            return doctorViewModel == null ? NotFound() : View(doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel doctorViewModel, string scheduleIdsJson, string reviewIdsJson, string appointmentIdsJson)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Id != doctorViewModel.Id)
                return Forbid();

            try
            {
                var existingDoctor = await _doctorService.GetDoctorByIdAsync(doctorViewModel.Id);
                if (existingDoctor == null)
                {
                    ModelState.AddModelError("", "Doctor not found.");
                    return View(doctorViewModel);
                }

                doctorViewModel.DepartmentId = existingDoctor.DepartmentId;
                doctorViewModel.DepartmentName = existingDoctor.DepartmentName;
                doctorViewModel.ScheduleIds = JsonSerializer.Deserialize<List<int>>(scheduleIdsJson) ?? new List<int>();
                doctorViewModel.ReviewIds = JsonSerializer.Deserialize<List<int>>(reviewIdsJson) ?? new List<int>();
                doctorViewModel.AppointmentIds = JsonSerializer.Deserialize<List<int>>(appointmentIdsJson) ?? new List<int>();

                ModelState.Clear();
                if (!TryValidateModel(doctorViewModel))
                    return View(doctorViewModel);

                var success = await _doctorService.UpdateDoctorAsync(doctorViewModel);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update doctor profile.");
                    return View(doctorViewModel);
                }

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Edit action");
                ModelState.AddModelError("", "An error occurred while updating the profile.");
                return View(doctorViewModel);
            }
        }

        public async Task<IActionResult> MedicalHistory()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    _logger.LogWarning("No current user found");
                    return RedirectToAction("Login", "Account");
                }

                var doctorViewModel = await _doctorService.GetDoctorByIdAsync(currentUser.Id);
                if (doctorViewModel == null)
                {
                    _logger.LogWarning($"Doctor profile not found for ID {currentUser.Id}");
                    TempData["ErrorMessage"] = "Doctor profile not found.";
                    return RedirectToAction(nameof(Profile));
                }

                var records = await _medicalRecordRepository.GetAllAsync() ?? new List<MedicalRecordDto>();
                var doctorRecords = records.Where(r => r.DoctorId == currentUser.Id).ToList();

                var allAppointments = await _appointmentRepository.GetAllAsync() ?? new List<AppointmentDto>();
                var doctorAppointments = allAppointments.Where(a => a.DoctorId == currentUser.Id).ToList();

                var appointmentsWithPatients = new List<AppointmentDto>();
                foreach (var appointment in doctorAppointments)
                {
                    var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
                    if (patient != null)
                        ViewData[$"PatientName_{appointment.PatientId}"] = patient.Name;
                    appointmentsWithPatients.Add(appointment);
                }

                var recentPatients = await Task.WhenAll(
                    doctorRecords
                        .GroupBy(r => r.PatientId)
                        .Select(async g => {
                            var patient = await _patientRepository.GetByIdAsync(g.Key);
                            return new PatientSummaryViewModel
                            {
                                Id = g.Key,
                                Name = patient?.Name ?? $"Patient {g.Key}",
                                LastVisit = g.Max(r => r.CreatedAt ?? DateTime.MinValue),
                                VisitCount = g.Count()
                            };
                        }));

                var viewModel = new DoctorMedicalHistoryViewModel
                {
                    DoctorName = doctorViewModel.Name,
                    DepartmentName = doctorViewModel.DepartmentName,
                    MedicalRecords = doctorRecords.OrderByDescending(r => r.CreatedAt).ToList(),
                    RecentPatients = recentPatients.OrderByDescending(p => p.LastVisit).ToList(),
                    UpcomingAppointments = appointmentsWithPatients
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in MedicalHistory action");
                TempData["ErrorMessage"] = "An error occurred while loading the medical history.";
                return RedirectToAction(nameof(Profile));
            }
        }

        public async Task<IActionResult> ViewRecord(int id)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                    return RedirectToAction("Login", "Account");

                var records = await _medicalRecordRepository.GetAllAsync();
                var record = records?.FirstOrDefault(r => r.Id == id && r.DoctorId == currentUser.Id);
                if (record == null)
                    return NotFound("Medical record not found or you don't have permission to view it.");

                var patient = await _patientRepository.GetByIdAsync(record.PatientId);
                if (patient != null)
                    ViewData["PatientName"] = patient.Name;

                var procedure = await _procedureRepository.GetByIdAsync(record.ProcedureId);
                if (procedure != null)
                    ViewData["ProcedureName"] = procedure.Name;

                var doctor = await _doctorService.GetDoctorByIdAsync(record.DoctorId);
                if (doctor != null)
                {
                    ViewData["DoctorName"] = doctor.Name;
                    ViewData["DoctorDepartment"] = doctor.DepartmentName;
                }

                return View(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ViewRecord action");
                ModelState.AddModelError("", "An error occurred while loading the medical record.");
                return RedirectToAction(nameof(MedicalHistory));
            }
        }
    }
} 