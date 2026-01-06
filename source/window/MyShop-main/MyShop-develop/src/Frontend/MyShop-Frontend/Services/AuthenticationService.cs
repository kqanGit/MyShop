using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Net.Http;
using System.Text;
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
            throw new HttpRequestException(
                $"Login failed: {(int)response.StatusCode} {response.ReasonPhrase}\n{rawJson}");
        }

        AuthResponseDto? result;
        try
        {
            result = JsonSerializer.Deserialize<AuthResponseDto>(rawJson, _jsonOptions);
        }
        catch (JsonException jex)
        {
            throw new InvalidOperationException(
                $"Login response is not valid JSON for {nameof(AuthResponseDto)}.\n{rawJson}", jex);
        }

        var token = result?.Token;
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                $"Login succeeded but token was missing/empty. Raw response:\n{rawJson}");
        }

        _tokenStore.SetAccessToken(token);
        _tokenStore.SetUserInfo(result!.Username, result.Role);

        return token;
    }
}