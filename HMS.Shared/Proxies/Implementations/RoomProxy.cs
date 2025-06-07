using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Repositories.Interfaces;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HMS.Shared.Proxies.Implementations;

public class RoomProxy : IRoomRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = Config._base_api_url;
    private readonly string _token;

    public RoomProxy(string token)
    {
        this._httpClient = new HttpClient { BaseAddress = new Uri(this._baseUrl) };
        this._token = token;
    }

    public RoomProxy(HttpClient httpClient, string token)
    {
        this._httpClient = httpClient;
        this._token = token;
    }
    private void AddAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._token);
    }
    public async Task<List<RoomDto>> GetAllAsync()
    {
        try
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "room");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RoomDto>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            })!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting all rooms: {ex.Message}");
            throw;
        }
    }

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        try
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"room/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } // without this, the enum values will not match
            };
            return JsonSerializer.Deserialize<RoomDto>(responseBody, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room by ID: {ex.Message}");
            throw;
        }
    }

    public async Task AddAsync(RoomDto room)
    {
        try
        {
            AddAuthorizationHeader();
            string departmentJson = JsonSerializer.Serialize(room, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });

            StringContent content = new StringContent(departmentJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "room", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding room: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(RoomDto room)
    {
        try
        {
            AddAuthorizationHeader();
            string appointmentJson = JsonSerializer.Serialize(room, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            });
            StringContent content = new StringContent(appointmentJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"room/{room.Id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return;

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating room: {ex.Message}");
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"room/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return;

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting room: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"room/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking if Room exists: {ex.Message}");
            throw;
        }
    }

    
}