using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs.Patient;
using System;
using System.Threading.Tasks;
using HMS.WebClient.Services;
using HMS.Shared.Enums;
using HMS.WebClient.Attributes;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;
        private readonly AuthService _authService;

        public PatientController(
            PatientService patientService,
            AuthService authService)
        {
            _patientService = patientService;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
        }

        // View profile information
        public async Task<IActionResult> Profile()
        {
            try
            {
                var patientId = _authService.GetUserId();
                if (patientId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var patient = await _patientService.GetPatientByIdAsync(patientId.Value);
                return View(patient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Patient not found. Please make sure you are logged in with a valid patient account.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while loading the profile: {ex.Message}");
                return View(new PatientDto());
            }
        }

        // Display edit form
        public async Task<IActionResult> Edit()
        {
            try
            {
                var patientId = _authService.GetUserId();
                if (patientId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var patient = await _patientService.GetPatientByIdAsync(patientId.Value);

                // Store original patient data in TempData for comparison during update
                TempData["OriginalPatient"] = System.Text.Json.JsonSerializer.Serialize(patient);

                return View(patient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Patient not found. Please make sure you are logged in with a valid patient account.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while loading the profile for editing: {ex.Message}");
                return View(new PatientDto());
            }
        }

        // Process edit form submission
        [HttpPost]
        public async Task<IActionResult> Edit(PatientDto model)
        {
            try
            {
                // Remove Password validation error since we don't require it in the form
                if (ModelState.ContainsKey("Password"))
                {
                    ModelState.Remove("Password");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var patientId = _authService.GetUserId();
                if (patientId == null || patientId.Value != model.Id)
                {
                    TempData["ErrorMessage"] = "You can only update your own profile.";
                    return RedirectToAction(nameof(Profile));
                }

                // Get the original patient data to compare and retain unchanged values
                var originalPatientJson = TempData["OriginalPatient"] as string;

                // If we couldn't get the original patient from TempData, fetch it from the database
                PatientDto originalPatient;
                if (string.IsNullOrEmpty(originalPatientJson))
                {
                    originalPatient = await _patientService.GetPatientByIdAsync(model.Id);
                }
                else
                {
                    originalPatient = System.Text.Json.JsonSerializer.Deserialize<PatientDto>(originalPatientJson);
                }

                // Create update DTO with only changed fields
                var updateDto = new PatientUpdateDto
                {
                    Id = model.Id,
                    // Required fields that shouldn't change or are hidden
                    Email = originalPatient.Email,
                    Password = originalPatient.Password ?? string.Empty, // Keep original password
                    CNP = originalPatient.CNP,
                    Role = originalPatient.Role.ToString(),
                    BirthDate = originalPatient.BirthDate,

                    // Fields that can be updated
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    BloodType = model.BloodType ?? originalPatient.BloodType ?? string.Empty,
                    EmergencyContact = model.EmergencyContact ?? originalPatient.EmergencyContact ?? string.Empty,
                    Allergies = model.Allergies ?? originalPatient.Allergies ?? string.Empty,
                    Weight = model.Weight,
                    Height = model.Height,
                    Address = model.Address ?? originalPatient.Address ?? string.Empty
                };

                var success = await _patientService.UpdatePatientAsync(updateDto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update your profile. Please try again.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }
    }
}