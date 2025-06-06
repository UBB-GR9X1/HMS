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
    public class UserProxy : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor cu HttpClient + token
        public UserProxy(HttpClient httpClient, string token)
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
        public UserProxy(string token)
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

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "user");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<UserDto>>(responseBody, _jsonOptions) ?? new List<UserDto>();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"user/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(responseBody, _jsonOptions);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"user/email/{email}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(responseBody, _jsonOptions);
        }

        public async Task<UserDto> AddAsync(UserDto user)
        {
            AddAuthorizationHeader();
            string userJson = JsonSerializer.Serialize(user, _jsonOptions);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "user", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(responseBody, _jsonOptions)!;
        }

        public async Task<bool> UpdateAsync(UserDto user)
        {
            AddAuthorizationHeader();

            string userJson = JsonSerializer.Serialize(user, _jsonOptions);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"user/{user.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();

            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"user/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
