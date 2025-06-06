using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HMS.Shared.DTOs;

namespace HMS.Shared.Proxies.Implementations
{
    public class ProcedureProxy : IProcedureRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = Config._base_api_url;
        private readonly string _token;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProcedureProxy(HttpClient httpClient, string token)
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

        public ProcedureProxy(string token)
        {
            this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<IEnumerable<ProcedureDto>> GetAllAsync()
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "procedure");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var procedures = JsonSerializer.Deserialize<IEnumerable<ProcedureDto>>(responseBody, _jsonOptions);

            return procedures ?? new List<ProcedureDto>();
        }

        public async Task<ProcedureDto?> GetByIdAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}procedure/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ProcedureDto>(responseBody, _jsonOptions);
        }

        public async Task<ProcedureDto> AddAsync(ProcedureDto procedure)
        {
            AddAuthorizationHeader();
            string procedureJson = JsonSerializer.Serialize(procedure, _jsonOptions);
            StringContent content = new StringContent(procedureJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}procedure", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProcedureDto>(responseBody, _jsonOptions)!;
        }

        public async Task<bool> UpdateAsync(ProcedureDto procedure)
        {
            AddAuthorizationHeader();
            string procedureJson = JsonSerializer.Serialize(procedure, _jsonOptions);
            StringContent content = new StringContent(procedureJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"{_baseUrl}procedure/{procedure.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}procedure/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
