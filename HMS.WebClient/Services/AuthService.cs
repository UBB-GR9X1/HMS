using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HMS.Shared.DTOs;
using HMS.Shared.Enums;

namespace HMS.WebClient.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string TokenKey = "JWTToken";
        private const string UserKey = "CurrentUser";

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5203/api/";
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
        }

        public int? GetUserId()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                return user.Id;
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        public async Task<UserWithTokenDto?> Login(string email, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"User/login?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                try
                {
                    var userWithToken = JsonSerializer.Deserialize<UserWithTokenDto>(jsonContent, options);

                    if (userWithToken == null || string.IsNullOrEmpty(userWithToken.Token))
                    {
                        return null;
                    }

                    // Clear any existing session data
                    _httpContextAccessor.HttpContext?.Session.Clear();

                    // Store token and user data in session
                    _httpContextAccessor.HttpContext?.Session.SetString(TokenKey, userWithToken.Token);
                    _httpContextAccessor.HttpContext?.Session.SetString(UserKey, jsonContent);

                    return userWithToken;
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Register(UserDto userDto)
        {
            try
            { 
                var json = JsonSerializer.Serialize(userDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("User", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public UserWithTokenDto? GetCurrentUser()
        {
            try
            {
                var userJson = _httpContextAccessor.HttpContext?.Session.GetString(UserKey);

                if (string.IsNullOrEmpty(userJson))
                {
                    return null;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                try
                {
                    return JsonSerializer.Deserialize<UserWithTokenDto>(userJson, options);
                }
                catch
                {
                    // Session data might be corrupted, clear it
                    _httpContextAccessor.HttpContext?.Session.Remove(UserKey);
                    _httpContextAccessor.HttpContext?.Session.Remove(TokenKey);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(TokenKey);
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(TokenKey);
            _httpContextAccessor.HttpContext?.Session.Remove(UserKey);
        }

        public HttpClient CreateAuthorizedClient()
        {
            var client = new HttpClient();
            var token = GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            client.BaseAddress = _httpClient.BaseAddress;
            return client;
        }
    }
}