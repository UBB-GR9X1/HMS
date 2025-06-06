using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IProcedureRepository _procedureRepository;
        private readonly IRoomRepository _roomRepository;

        public AppointmentController(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IProcedureRepository procedureRepository,
            IRoomRepository roomRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _procedureRepository = procedureRepository;
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Retrieves all appointments.
        /// </summary>
        /// <returns>List of appointments.</returns>
        /// <response code="200">Returns the list of appointments.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var appDtos = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                ProcedureId = a.ProcedureId,
                RoomId = a.RoomId,
                DateTime = a.DateTime
            });
            return Ok(appDtos);
        }

        /// <summary>
        /// Retrieves a specific appointment by id.
        /// </summary>
        /// <param name="id">Appointment's id.</param>
        /// <returns>The requested appointment.</returns>
        /// <response code="200">Returns the requested appointment.</response>
        /// <response code="404">If the appointment is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Appointment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="dto">Appointment DTO to create.</param>
        /// <returns>The created appointment.</returns>
        /// <response code="201">Returns the newly created appointment.</response>
        /// <response code="400">If the appointment object is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Appointment), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] AppointmentDto dto)
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

            var room = await _roomRepository.GetByIdAsync(dto.RoomId);
            if (room == null)
                return BadRequest($"Room with ID {dto.RoomId} not found.");

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                Patient = patient,
                DoctorId = dto.DoctorId,
                Doctor = doctor,
                ProcedureId = dto.ProcedureId,
                Procedure = procedure,
                RoomId = dto.RoomId,
                Room = room,
                DateTime = dto.DateTime
            };

            var createdAppointment = await _appointmentRepository.AddAsync(appointment);
            return CreatedAtAction(nameof(GetById), new { id = createdAppointment.Id }, createdAppointment);
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="id">Appointment's id.</param>
        /// <param name="dto">Appointment DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If appointment not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto dto)
        {
            var existing = await _appointmentRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                return BadRequest($"Patient with ID {dto.PatientId} not found.");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var procedure = await _procedureRepository.GetByIdAsync(dto.ProcedureId);
            if (procedure == null)
                return BadRequest($"Procedure with ID {dto.ProcedureId} not found.");

            var room = await _roomRepository.GetByIdAsync(dto.RoomId);
            if (room == null)
                return BadRequest($"Room with ID {dto.RoomId} not found.");

            existing.PatientId = dto.PatientId;
            existing.Patient = patient;
            existing.DoctorId = dto.DoctorId;
            existing.Doctor = doctor;
            existing.ProcedureId = dto.ProcedureId;
            existing.Procedure = procedure;
            existing.RoomId = dto.RoomId;
            existing.Room = room;
            existing.DateTime = dto.DateTime;

            await _appointmentRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Deletes an appointment.
        /// </summary>
        /// <param name="id">Appointment's id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If appointment not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentRepository.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
