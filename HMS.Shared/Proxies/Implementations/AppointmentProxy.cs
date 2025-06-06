using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using HMS.Shared.DTOs;

namespace HMS.Shared.Proxies.Implementations
{
    public class AppointmentProxy : IAppointmentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public AppointmentProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Ignore,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public AppointmentProxy(string token)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Ignore,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<AppointmentDto> AddAsync(AppointmentDto appointment)
        {
            try
            {
                AddAuthorizationHeader();
                string appointmentJson = JsonSerializer.Serialize(appointment, _jsonOptions);
                StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "appointment", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AppointmentDto>(responseBody, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"appointment/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting appointment: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "appointment");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<AppointmentDto>>(responseBody, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all appointments: {ex.Message}");
                throw;
            }
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"appointment/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AppointmentDto>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting appointment by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(AppointmentDto appointment)
        {
            try
            {
                AddAuthorizationHeader();
                string appointmentJson = JsonSerializer.Serialize(appointment, _jsonOptions);
                StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"appointment/{appointment.Id}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating appointment: {ex.Message}");
                throw;
            }
        }
    }
}