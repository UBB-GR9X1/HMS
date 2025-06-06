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
    public class AdminProxy : IAdminRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;

        public AdminProxy(string token)
        {
            this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
            this._token = token;
        }

        public AdminProxy(HttpClient httpClient, string token)
        {
            this._httpClient = httpClient;
            this._token = token;
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
        }

        public async Task<AdminDto> AddAsync(Admin admin)
        {
            AddAuthorizationHeader();

            string adminJson = JsonSerializer.Serialize(admin, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });
            StringContent content = new StringContent(adminJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "admin", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            AdminDto createdAdmin = JsonSerializer.Deserialize<AdminDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });

            if (createdAdmin == null)
            {
                throw new Exception("Failed to deserialize created user.");
            }
            return createdAdmin;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"admin/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting admin: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<AdminDto>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();
                HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "appointment");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<AdminDto>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                })!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all admins: {ex.Message}");
                throw;
            }
        }

        public async Task<AdminDto?> GetByEmailAsync(string email)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"admin/email/{email}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            AdminDto? admin = JsonSerializer.Deserialize<AdminDto>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });
            return admin;
        }

        public async Task<AdminDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"admin/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            };
            AdminDto? admin = JsonSerializer.Deserialize<AdminDto>(responseBody, options);
            return admin;
        }

        public async Task<bool> UpdateAsync(Admin admin)
        {
            AddAuthorizationHeader();

            string adminJson = JsonSerializer.Serialize(admin, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            });

            StringContent content = new StringContent(adminJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"admin/{admin.Id}", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true; // Update successful
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Invalid admin data provided.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false; // Admin not found
            }
            else
            {
                throw new Exception("Failed to update admin.");
            }
        }
    }
}
