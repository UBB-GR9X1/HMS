using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HMS.Shared.Proxies.Implementations
{
    public class LoggerProxy : ILoggerRepository
    {
        private readonly HttpClient _httpClient;
        private static readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        // Constructor cu HttpClient + token
        public LoggerProxy(HttpClient httpClient, string token)
        {
            _httpClient = httpClient;
            _token = token;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };
        }

        // Constructor doar cu token
        public LoggerProxy(string token)
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

        public async Task<IEnumerable<LogDto>> GetAllAsync()
        {
            try
            {
                AddAuthorizationHeader();

                HttpResponseMessage response = await this._httpClient.GetAsync(_baseUrl + "log");
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    if (document.RootElement.TryGetProperty("$values", out JsonElement valuesElement))
                    {
                        string valuesJson = valuesElement.GetRawText();
                        var logs = JsonSerializer.Deserialize<List<LogDto>>(valuesJson, _jsonOptions);
                        return logs ?? new List<LogDto>();
                    }
                    else
                    {
                        var logs = JsonSerializer.Deserialize<List<LogDto>>(json, _jsonOptions);
                        return logs ?? new List<LogDto>();
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetAllAsync: {exception.Message}");
                return new List<LogDto>();
            }
        }

        public async Task<LogDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"log/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LogDto>(responseBody, _jsonOptions);
        }

        public async Task<LogDto> AddAsync(LogDto log)
        {
            AddAuthorizationHeader();
            string logJson = JsonSerializer.Serialize(log, _jsonOptions);
            StringContent content = new StringContent(logJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "log", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LogDto>(responseBody, _jsonOptions)!;
        }

        public async Task<bool> UpdateAsync(LogDto log)
        {
            AddAuthorizationHeader();
            string logJson = JsonSerializer.Serialize(log, _jsonOptions);
            StringContent content = new StringContent(logJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"log/{log.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"log/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
