using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs.Patient;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>List of patients.</returns>
        /// <response code="200">Returns the list of patients.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientRepository.GetAllAsync();
            var dtos = patients.Select(p => new PatientDto
            {
                Id = p.Id,
                Email = p.Email,
                Password = p.Password,
                Name = p.Name,
                CNP = p.CNP,
                PhoneNumber = p.PhoneNumber,
                Role = p.Role,
                BloodType = p.BloodType.ToString(),
                EmergencyContact = p.EmergencyContact,
                Allergies = p.Allergies,
                Weight = p.Weight,
                Height = p.Height,
                BirthDate = p.BirthDate,
                Address = p.Address,
                ReviewIds = p.Reviews.Select(r => r.Id).ToList(),
                AppointmentIds = p.Appointments.Select(a => a.Id).ToList(),
                MedicalRecordIds = p.MedicalRecords.Select(m => m.Id).ToList()
            });

            return Ok(dtos);
        }

        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        /// <param name="id">Patient's ID.</param>
        /// <returns>The requested patient.</returns>
        /// <response code="200">Returns the requested patient.</response>
        /// <response code="404">If the patient is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();

            var dto = new PatientDto
            {
                Id = patient.Id,
                Email = patient.Email,
                Password = patient.Password,
                Name = patient.Name,
                CNP = patient.CNP,
                PhoneNumber = patient.PhoneNumber,
                Role = patient.Role,
                BloodType = patient.BloodType.ToString(),
                EmergencyContact = patient.EmergencyContact,
                Allergies = patient.Allergies,
                Weight = patient.Weight,
                Height = patient.Height,
                BirthDate = patient.BirthDate,
                Address = patient.Address,
                ReviewIds = patient.Reviews.Select(r => r.Id).ToList(),
                AppointmentIds = patient.Appointments.Select(a => a.Id).ToList(),
                MedicalRecordIds = patient.MedicalRecords.Select(m => m.Id).ToList()
            };

            return Ok(dto);
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="dto">DTO containing patient data.</param>
        /// <returns>The created patient.</returns>
        /// <response code="201">Returns the newly created patient.</response>
        /// <response code="400">If the input data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Patient), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPatient = await _patientRepository.AddAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">ID of the patient to update.</param>
        /// <param name="dto">DTO containing updated patient data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If the input data is invalid.</response>
        /// <response code="404">If the patient was not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto dto)
        {
            var existing = await _patientRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Email = dto.Email;
            existing.Password = dto.Password;
            existing.Name = dto.Name;
            existing.CNP = dto.CNP;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.Role = Enum.Parse<HMS.Shared.Enums.UserRole>(dto.Role);
            existing.BloodType = Enum.Parse<HMS.Shared.Enums.BloodType>(dto.BloodType);
            existing.EmergencyContact = dto.EmergencyContact;
            existing.Allergies = dto.Allergies;
            existing.Weight = dto.Weight;
            existing.Height = dto.Height;
            existing.BirthDate = dto.BirthDate;
            existing.Address = dto.Address;

            var success = await _patientRepository.UpdateAsync(existing);
            if (!success) return BadRequest("Failed to update the patient.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a patient.
        /// </summary>
        /// <param name="id">ID of the patient to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If the patient was not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientRepository.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
