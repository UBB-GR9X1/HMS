using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using HMS.Shared.DTOs;

namespace HMS.Shared.Proxies.Implementations;

public class ShiftProxy : IShiftRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = Config._base_api_url;
    private readonly string _token;
    private readonly JsonSerializerOptions _jsonOptions;

    // Constructor cu HttpClient + token
    public ShiftProxy(HttpClient httpClient, string token)
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
    public ShiftProxy(string token)
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

    public async Task<IEnumerable<ShiftDto>> GetAllAsync()
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "shift");
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ShiftDto>>(responseBody, _jsonOptions) ?? new List<ShiftDto>();
    }

    public async Task<ShiftDto?> GetByIdAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"shift/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ShiftDto>(responseBody, _jsonOptions);
    }

    public async Task<ShiftDto> AddAsync(ShiftDto shift)
    {
        AddAuthorizationHeader();
        string shiftJson = JsonSerializer.Serialize(shift, _jsonOptions);
        StringContent content = new StringContent(shiftJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "shift", content);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ShiftDto>(json, _jsonOptions)!;
    }

    public async Task<bool> UpdateAsync(ShiftDto shift)
    {
        AddAuthorizationHeader();
        string shiftJson = JsonSerializer.Serialize(shift, _jsonOptions);
        StringContent content = new StringContent(shiftJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"shift/{shift.Date}", content);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        AddAuthorizationHeader();
        HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"shift/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;

        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}