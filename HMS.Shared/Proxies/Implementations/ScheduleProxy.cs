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
    public class ScheduleProxy : IScheduleRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor cu HttpClient + token
        public ScheduleProxy(HttpClient httpClient, string token)
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
        public ScheduleProxy(string token)
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

        public async Task<ScheduleDto> AddAsync(ScheduleDto schedule)
        {
            AddAuthorizationHeader();
            string scheduleJson = JsonSerializer.Serialize(schedule, _jsonOptions);
            StringContent content = new StringContent(scheduleJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "schedule", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ScheduleDto>(responseBody, _jsonOptions)!;
        }

        public async Task<bool> DeleteAsync(int doctorId, int shiftId)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"schedule/{doctorId}/{shiftId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "schedule");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ScheduleDto>>(responseBody, _jsonOptions) ?? new List<ScheduleDto>();
        }

        public async Task<ScheduleDto?> GetByIdsAsync(int doctorId, int shiftId)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"schedule/{doctorId}/{shiftId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ScheduleDto>(responseBody, _jsonOptions);
        }

        public async Task<bool> UpdateAsync(ScheduleDto schedule)
        {
            AddAuthorizationHeader();
            string scheduleJson = JsonSerializer.Serialize(schedule, _jsonOptions);
            StringContent content = new StringContent(scheduleJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"schedule/{schedule.DoctorId}/{schedule.ShiftId}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
} 