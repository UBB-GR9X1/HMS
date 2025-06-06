using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IShiftRepository _shiftRepository;

        public ScheduleController(IScheduleRepository scheduleRepository, IDoctorRepository doctorRepository, IShiftRepository shiftRepository)
        {
            _scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
            _shiftRepository = shiftRepository;
        }

        /// <summary>
        /// Retrieves all schedules.
        /// </summary>
        /// <returns>List of schedules.</returns>
        /// <response code="200">Returns the list of schedules.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Schedule>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleRepository.GetAllAsync();

            var scheduleDtos = schedules.Select(s => new ScheduleDto
            {
                ShiftId = s.ShiftId,
                DoctorId = s.DoctorId
            });

            return Ok(schedules);
        }

        /// <summary>
        /// Retrieves a specific schedule by doctorId and shiftId.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>The requested schedule.</returns>
        /// <response code="200">Returns the requested schedule.</response>
        /// <response code="404">If the schedule is not found.</response>
        [HttpGet("{doctorId}/{shiftId}")]
        [Authorize]
        [ProducesResponseType(typeof(Schedule), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIds(int doctorId, int shiftId)
        {
            var schedule = await _scheduleRepository.GetByIdsAsync(doctorId, shiftId);
            if (schedule == null) return NotFound();
            return Ok(schedule);
        }

        /// <summary>
        /// Creates a new schedule.
        /// </summary>
        /// <param name="dto">Schedule DTO to create.</param>
        /// <returns>The created schedule.</returns>
        /// <response code="201">Returns the newly created schedule.</response>
        /// <response code="400">If the schedule object is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Schedule), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ScheduleDto dto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var shift = await _shiftRepository.GetByIdAsync(dto.ShiftId);
            if (shift == null)
                return BadRequest($"Shift with ID {dto.ShiftId} not found.");

            var schedule = new Schedule
            {
                DoctorId = dto.DoctorId,
                Doctor = doctor,
                ShiftId = dto.ShiftId,
                Shift = shift
            };

            var created = await _scheduleRepository.AddAsync(schedule);
            return CreatedAtAction(nameof(GetByIds), new { doctorId = created.DoctorId, shiftId = created.ShiftId }, created);
        }

        /// <summary>
        /// Updates an existing schedule.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <param name="dto">Schedule DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If schedule not found.</response>
        [HttpPut("{doctorId}/{shiftId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int doctorId, int shiftId, [FromBody] ScheduleDto dto)
        {
            if (doctorId != dto.DoctorId || shiftId != dto.ShiftId)
                return BadRequest("Doctor ID or Shift ID mismatch.");

            var existing = await _scheduleRepository.GetByIdsAsync(doctorId, shiftId);
            if (existing == null)
                return NotFound();

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var shift = await _shiftRepository.GetByIdAsync(dto.ShiftId);
            if (shift == null)
                return BadRequest($"Shift with ID {dto.ShiftId} not found.");

            existing.DoctorId = dto.DoctorId;
            existing.Doctor = doctor;
            existing.ShiftId = dto.ShiftId;
            existing.Shift = shift;

            await _scheduleRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Deletes a schedule.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If schedule not found.</response>
        [HttpDelete("{doctorId}/{shiftId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int doctorId, int shiftId)
        {
            var result = await _scheduleRepository.DeleteAsync(doctorId, shiftId);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
