using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    public class MedicalRecordProxy : IMedicalRecordRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public MedicalRecordProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public MedicalRecordProxy(string token)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<MedicalRecordDto> AddAsync(MedicalRecordDto medicalRecord)
        {
            AddAuthorizationHeader();
            string recordJson = JsonSerializer.Serialize(medicalRecord, _jsonOptions);
            StringContent content = new StringContent(recordJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "medicalrecord", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MedicalRecordDto>(json, _jsonOptions)!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"medicalrecord/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "medicalrecord");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var wrapper = JsonSerializer.Deserialize<MedicalRecordResponseDto>(responseBody, _jsonOptions);
                return wrapper?.Records ?? new List<MedicalRecordDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting medical records: {ex.Message}");
                throw;
            }
        }

        public async Task<MedicalRecordDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"medicalrecord/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MedicalRecordDto>(responseBody, _jsonOptions);
        }

        public async Task<bool> UpdateAsync(MedicalRecordDto medicalRecord)
        {
            AddAuthorizationHeader();
            string recordJson = JsonSerializer.Serialize(medicalRecord, _jsonOptions);
            StringContent content = new StringContent(recordJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"medicalrecord/{medicalRecord.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
} 