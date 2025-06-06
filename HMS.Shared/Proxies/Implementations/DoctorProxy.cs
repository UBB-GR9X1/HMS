using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace HMS.Shared.Proxies.Implementations
{
    public class DoctorProxy : IDoctorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor cu HttpClient + token
        public DoctorProxy(HttpClient httpClient, string token)
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

        // Constructor doar cu token
        public DoctorProxy(string token)
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

        public async Task<DoctorDto> AddAsync(DoctorDto doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, _jsonOptions);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "doctor", content);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DoctorDto>(json, _jsonOptions)!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"doctor/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "doctor");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<DoctorDto>>(responseBody, _jsonOptions)!;
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"doctor/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DoctorDto>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting doctor with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(DoctorDto doctor)
        {
            AddAuthorizationHeader();
            string doctorJson = JsonSerializer.Serialize(doctor, _jsonOptions);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"doctor/{doctor.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
