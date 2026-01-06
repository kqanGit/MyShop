using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AuthenticationService(HttpClient httpClient, ITokenStore tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
        }

        public async Task<AuthResponseDto?> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            var request = new LoginRequestDto
            {
                Username = username,
                Password = password
            };

            const string relativeUrl = "api/auth/login";

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, relativeUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(httpRequest, ct);
            var rawJson = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            AuthResponseDto? result;
            try
            {
                result = JsonSerializer.Deserialize<AuthResponseDto>(rawJson, _jsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }

            var token = result?.Token;
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            _tokenStore.SetAccessToken(token);
            _tokenStore.SetUserInfo(result!.Username, result.Role);

            return result;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
        {
            const string relativeUrl = "api/auth/register";

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, relativeUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(httpRequest, ct);
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawJson = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<AuthResponseDto>(rawJson, _jsonOptions);
        }

        public async Task<bool> LogoutAsync(CancellationToken ct = default)
        {
            const string relativeUrl = "api/auth/logout";

            var request = new LogoutRequestDto();
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, relativeUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(httpRequest, ct);
            
            if (response.IsSuccessStatusCode)
            {
                _tokenStore.SetAccessToken(string.Empty);
                return true;
            }

            return false;
        }
    }
}