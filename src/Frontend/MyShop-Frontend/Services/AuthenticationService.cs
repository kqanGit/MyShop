using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static MyShop_Frontend.Contracts.Dtos.AuthDto;

namespace MyShop_Frontend.Services;

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

    public async Task<string?> LoginAsync(string username, string password)
    {
        return await LoginAsync(username, password, CancellationToken.None);
    }

    private async Task<string?> LoginAsync(string username, string password, CancellationToken ct)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        using var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request, ct);
        var rawJson = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Login failed: {(int)response.StatusCode} {response.ReasonPhrase}");
        }

        var result = JsonSerializer.Deserialize<AuthResponseDto>(rawJson, _jsonOptions);

        var token = result?.Token;
        if (string.IsNullOrWhiteSpace(token))
            return null;

        // Lưu token và thông tin user
        _tokenStore.SetAccessToken(token);
        _tokenStore.SetUserInfo(result!.Username, result.Role);

        return token;
    }
}