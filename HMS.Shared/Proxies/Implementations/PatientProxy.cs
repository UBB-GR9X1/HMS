using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HMS.Shared.DTOs.Patient;

namespace HMS.Shared.Proxies.Implementations
{
    public class PatientProxy : IPatientRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public PatientProxy(HttpClient httpClient)
        {
            this._http_client = httpClient;
        }

        // Constructor doar cu token (creează HttpClient cu BaseAddress)
        public PatientProxy(string token)
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

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "patient");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<PatientDto>>(responseBody, _jsonOptions) ?? new List<PatientDto>();
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}patient/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            PatientDto? patient = JsonSerializer.Deserialize<PatientDto>(responseBody, _jsonOptions);

            if (patient == null)
                throw new Exception($"No patient found with user id {id}");

            return patient;
        }

        public async Task<PatientDto> AddAsync(PatientDto patient)
        {
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(patient, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                }),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(this.s_base_api_url + "patient", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Body: {error}");
            }

            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            Patient patient_response = JsonSerializer.Deserialize<Patient>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() } // without this, the enum values will not match

            });

            return patient_response;
        }

        public async Task<bool> UpdateAsync(PatientUpdateDto patient, int id)
        {
            AddAuthorizationHeader();
            string jsonContent = JsonSerializer.Serialize(patient, _jsonOptions);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            Debug.WriteLine(jsonContent);
            HttpResponseMessage response = await _httpClient.PutAsync($"{_baseUrl}patient/{id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}patient/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
