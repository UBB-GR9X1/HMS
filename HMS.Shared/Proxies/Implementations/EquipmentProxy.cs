using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;

namespace HMS.Shared.Proxies.Implementations;

public class EquipmentProxy : IEquipmentRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = Config._base_api_url;
    private readonly string _token;
    private readonly JsonSerializerOptions _jsonOptions;

    public EquipmentProxy(HttpClient httpClient, string token)
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

    public EquipmentProxy(string token)
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

    public async Task<List<EquipmentDto>> GetAllAsync()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "equipment");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EquipmentDto>>(responseBody, _jsonOptions)!;
    }

    public async Task<EquipmentDto?> GetByIdAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"equipment/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EquipmentDto>(responseBody, _jsonOptions);
    }

    public async Task AddAsync(EquipmentDto equipment)
    {
        AddAuthorizationHeader();
        string equipmentJson = JsonSerializer.Serialize(equipment, _jsonOptions);
        StringContent content = new StringContent(equipmentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "equipment", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(EquipmentDto equipment)
    {
        AddAuthorizationHeader();
        string equipmentJson = JsonSerializer.Serialize(equipment, _jsonOptions);
        StringContent content = new StringContent(equipmentJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"equipment/{equipment.Id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"equipment/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"equipment/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}