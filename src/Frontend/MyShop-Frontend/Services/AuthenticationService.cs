using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System;
using System.Diagnostics;
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
        // nếu interface của bạn có CancellationToken thì thêm vào signature,
        // còn không thì cứ dùng CancellationToken.None
        return await LoginAsync(username, password, CancellationToken.None);
    }

    // Overload nội bộ để có CancellationToken (tuỳ bạn có muốn expose ra interface không)
    private async Task<string?> LoginAsync(string username, string password, CancellationToken ct)
    {
        var request = new AuthDto.LoginRequest
        {
            Username = username,
            Password = password
        };

        using var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request, ct);
        var rawJson = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            // Bạn có thể parse error body nếu backend trả kiểu khác
            throw new HttpRequestException($"Login failed: {(int)response.StatusCode} {response.ReasonPhrase}\n{rawJson}");
        }

        var result = JsonSerializer.Deserialize<AuthResponseDto>(rawJson, _jsonOptions);

        var token = result?.Token; // đúng theo file bạn đang dùng result?.Token
        if (string.IsNullOrWhiteSpace(token))
            return null;

        // Lưu token vào TokenStore để ApiClient tự gắn Authorization: Bearer <token>
        _tokenStore.SetAccessToken(token);

        return token;
    }
}