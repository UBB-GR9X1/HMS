using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IProcedureRepository _procedureRepository;
        private readonly JsonSerializerOptions _jsonOptions;

        public MedicalRecordController(
            IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IProcedureRepository procedureRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _procedureRepository = procedureRepository;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Retrieves all medical records.
        /// </summary>
        /// <returns>List of medical records.</returns>
        /// <response code="200">Returns the list of medical records.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<MedicalRecord>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var records = await _medicalRecordRepository.GetAllAsync();
            return Ok(new
            {
                records = records.Select(r => new
                {
                    r.Id,
                    r.PatientId,
                    PatientName = r.Patient?.Name,
                    r.DoctorId,
                    DoctorName = r.Doctor?.Name,
                    DoctorDepartment = r.Doctor?.Department?.Name,
                    r.ProcedureId,
                    ProcedureName = r.Procedure?.Name,
                    ProcedureDepartment = r.Procedure?.Department?.Name,
                    r.Diagnosis,
                    r.CreatedAt
                })
            });
        }

        /// <summary>
        /// Retrieves a specific medical record by ID.
        /// </summary>
        /// <param name="id">Medical record's ID.</param>
        /// <returns>The requested medical record.</returns>
        /// <response code="200">Returns the medical record.</response>
        /// <response code="404">If medical record is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(MedicalRecord), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(id);
            if (record == null) return NotFound();

            return Ok(new
            {
                record.Id,
                record.PatientId,
                PatientName = record.Patient?.Name,
                record.DoctorId,
                DoctorName = record.Doctor?.Name,
                DoctorDepartment = record.Doctor?.Department?.Name,
                record.ProcedureId,
                ProcedureName = record.Procedure?.Name,
                ProcedureDepartment = record.Procedure?.Department?.Name,
                record.Diagnosis,
                record.CreatedAt
            });
        }

        /// <summary>
        /// Creates a new medical record.
        /// </summary>
        /// <param name="dto">MedicalRecord DTO to create.</param>
        /// <returns>The created medical record.</returns>
        /// <response code="201">Returns the newly created medical record.</response>
        /// <response code="400">If the request data is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(MedicalRecord), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] MedicalRecordDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                return BadRequest($"Patient with ID {dto.PatientId} not found.");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var procedure = await _procedureRepository.GetByIdAsync(dto.ProcedureId);
            if (procedure == null)
                return BadRequest($"Procedure with ID {dto.ProcedureId} not found.");

            var record = new MedicalRecord
            {
                PatientId = dto.PatientId,
                Patient = patient,
                DoctorId = dto.DoctorId,
                Doctor = doctor,
                ProcedureId = dto.ProcedureId,
                Procedure = procedure,
                Diagnosis = dto.Diagnosis,
                CreatedAt = dto.CreatedAt ?? System.DateTime.UtcNow
            };

            var createdRecord = await _medicalRecordRepository.AddAsync(record);
            return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, new
            {
                createdRecord.Id,
                createdRecord.PatientId,
                PatientName = createdRecord.Patient?.Name,
                createdRecord.DoctorId,
                DoctorName = createdRecord.Doctor?.Name,
                DoctorDepartment = createdRecord.Doctor?.Department?.Name,
                createdRecord.ProcedureId,
                ProcedureName = createdRecord.Procedure?.Name,
                ProcedureDepartment = createdRecord.Procedure?.Department?.Name,
                createdRecord.Diagnosis,
                createdRecord.CreatedAt
            });
        }

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        /// <param name="id">Medical record ID.</param>
        /// <param name="dto">MedicalRecord DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If medical record not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] MedicalRecordDto dto)
        {
            var existing = await _medicalRecordRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                return BadRequest($"Patient with ID {dto.PatientId} not found.");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var procedure = await _procedureRepository.GetByIdAsync(dto.ProcedureId);
            if (procedure == null)
                return BadRequest($"Procedure with ID {dto.ProcedureId} not found.");

            existing.PatientId = dto.PatientId;
            existing.Patient = patient;
            existing.DoctorId = dto.DoctorId;
            existing.Doctor = doctor;
            existing.ProcedureId = dto.ProcedureId;
            existing.Procedure = procedure;
            existing.Diagnosis = dto.Diagnosis;
            existing.CreatedAt = dto.CreatedAt ?? existing.CreatedAt;

            await _medicalRecordRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Deletes a medical record by ID.
        /// </summary>
        /// <param name="id">Medical record ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If medical record not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _medicalRecordRepository.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// Retrieves all medical records created by a specific doctor.
        /// </summary>
        /// <param name="doctorId">Doctor's ID.</param>
        /// <returns>List of medical records for the doctor.</returns>
        [HttpGet("by-doctor/{doctorId}")]
        [Authorize]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var records = await _medicalRecordRepository.GetAllAsync();
            var doctorRecords = records
                .Where(r => r.DoctorId == doctorId)
                .Select(r => new
                {
                    r.Id,
                    r.PatientId,
                    PatientName = r.Patient?.Name,
                    r.DoctorId,
                    DoctorName = r.Doctor?.Name,
                    DoctorDepartment = r.Doctor?.Department?.Name,
                    r.ProcedureId,
                    ProcedureName = r.Procedure?.Name,
                    ProcedureDepartment = r.Procedure?.Department?.Name,
                    r.Diagnosis,
                    r.CreatedAt
                });

            return Ok(new { records = doctorRecords });
        }
    }
}
