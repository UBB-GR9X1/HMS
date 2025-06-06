using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    public class CreateAccountProxy : ICreateAccountRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public CreateAccountProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public CreateAccountProxy(string token)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<Patient> createAccount(PatientCreateDto patientDto)
        {
            AddAuthorizationHeader();

            // 1. Check if user already exists by CNP or Email
            HttpResponseMessage usersResponse = await _httpClient.GetAsync(_baseUrl + "user");
            usersResponse.EnsureSuccessStatusCode();

            string usersBody = await usersResponse.Content.ReadAsStringAsync();
            IEnumerable<UserDto> users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(usersBody, _jsonOptions)
                                         ?? new List<UserDto>();

            bool exists = users.Any(u => u.CNP == patientDto.CNP || u.Email == patientDto.Email);
            if (exists)
                throw new AuthenticationException("User with the same CNP or email already exists.");

            // 2. Create user (POST)
            UserDto userDto = new UserDto
            {
                Id = 0, // Server will set
                Email = patientDto.Email,
                Password = patientDto.Password,
                Role = HMS.Shared.Enums.UserRole.Patient,
                Name = patientDto.Name,
                CNP = patientDto.CNP,
                PhoneNumber = patientDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            };

            string userJson = JsonSerializer.Serialize(userDto, _jsonOptions);
            StringContent userContent = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage createUserResponse = await _httpClient.PostAsync(_baseUrl + "user", userContent);
            createUserResponse.EnsureSuccessStatusCode();

            string createdUserJson = await createUserResponse.Content.ReadAsStringAsync();
            UserDto createdUser = JsonSerializer.Deserialize<UserDto>(createdUserJson, _jsonOptions)!;

            // 3. Compose and return Patient (in memory, not persisted here)
            Patient patient = new Patient
            {
                Id = createdUser.Id,
                BloodType = Enum.Parse<HMS.Shared.Enums.BloodType>(patientDto.BloodType),
                EmergencyContact = patientDto.EmergencyContact,
                Allergies = patientDto.Allergies,
                Weight = patientDto.Weight,
                Height = patientDto.Height,
                BirthDate = patientDto.BirthDate,
                Address = patientDto.Address,
            };

            return patient;
        }
    }
}
