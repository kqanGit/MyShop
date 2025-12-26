using MyShop_Frontend.Contracts;
using MyShop_Frontend.Contracts.Dtos;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static MyShop_Frontend.Contracts.Dtos.AuthDto;

namespace MyShop_Frontend.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://stunning-goggles-x5rv46v6674p26q4j-5126.app.github.dev/") // Thay port đúng của Backend Docker
        };
    }

    // Trong Services/AuthenticationService.cs
    public static string? CurrentToken { get; private set; }

    public async Task<string?> LoginAsync(string username, string password)
    {
        try
        {
           

            var request = new AuthDto.LoginRequest { Username = username, Password = password };

            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
            string rawJson = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"JSON phản hồi: {rawJson}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<AuthResponseDto>(rawJson, options);
                CurrentToken = result?.Token;
                return result?.Token;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi hệ thống: {ex.Message}");
        }
        return null;
    }
}