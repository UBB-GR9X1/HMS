using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Repositories.Interfaces;
using HMS.Backend.Utils;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Backend.Controllers
{
    /// <summary>
    /// Controller responsible for handling user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private TokenProvider tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="repository">The user repository instance.</param>
        public UserController(IUserRepository repository)
        {
            _repository = repository;
            tokenProvider = new TokenProvider();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of <see cref="UserDto"/> objects.</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _repository.GetAllAsync();

            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(MapToDto(user));
            }

            return Ok(dtos);
        }

        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The <see cref="UserDto"/> if found.</returns>
        /// <response code="200">Returns the requested user.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The <see cref="UserDto"/> if found.</returns>
        /// <response code="200">Returns the requested user.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("email/{email}")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                return NotFound();

            return Ok(MapToDto(user));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The user DTO to create.</param>
        /// <returns>The created user with HTTP 201 status.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the model state is invalid or the email is already in use.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUserWithEmail = await _repository.GetByEmailAsync(dto.Email);
            if (existingUserWithEmail != null)
                return BadRequest($"Email '{dto.Email}' is already in use.");

            var user = MapToEntity(dto);
            await _repository.AddAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, MapToDto(user));
        }

        // GET: api/user/login
        [HttpGet("login")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UserWithTokenDto>> Login([FromQuery] string email, [FromQuery] string password)
        {
            User user = await _repository.GetByEmailAsync(email);
            if (user == null)
                return NotFound("User not found");

            if (user.Password != password)
                return Unauthorized("Invalid password");

            var token = tokenProvider.Create(user.Id);

            return MapToDto(user, token);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the model state is invalid or ID mismatch occurs.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Id in URL and payload do not match");

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            var userWithEmail = await _repository.GetByEmailAsync(dto.Email);
            if (userWithEmail != null && userWithEmail.Id != id)
                return BadRequest($"Email '{dto.Email}' is already in use by another user.");

            existingUser.Email = dto.Email;
            existingUser.Password = dto.Password;
            existingUser.Role = dto.Role;
            existingUser.Name = dto.Name;
            existingUser.CNP = dto.CNP;
            existingUser.PhoneNumber = dto.PhoneNumber;
            existingUser.CreatedAt = dto.CreatedAt;

            await _repository.UpdateAsync(existingUser);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if deleted successfully.</returns>
        /// <response code="204">If the deletion was successful.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Maps a <see cref="User"/> entity to a <see cref="UserDto"/>.
        /// </summary>
        /// <param name="user">The user entity to map.</param>
        /// <returns>The mapped DTO.</returns>
        private UserDto MapToDto(User user) =>
            new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Name = user.Name,
                CNP = user.CNP,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };

        private UserWithTokenDto MapToDto(User user, string token) =>
            new UserWithTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Name = user.Name,
                CNP = user.CNP,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                Token = token
            };

        /// <summary>
        /// Maps a <see cref="UserDto"/> to a <see cref="User"/> entity.
        /// </summary>
        /// <param name="dto">The DTO to map.</param>
        /// <returns>The mapped entity.</returns>
        private User MapToEntity(UserDto dto) =>
            new User
            {
                Id = dto.Id,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                Name = dto.Name,
                CNP = dto.CNP,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = dto.CreatedAt
            };
    }
}
