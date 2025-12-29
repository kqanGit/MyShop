using MyShop_Frontend.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public sealed class ApiClient : IApiClient
    {
        private readonly HttpClient _http;
        private readonly ITokenStore _tokenStore;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient(HttpClient http, ITokenStore tokenStore)
        {
            _http = http;
            _tokenStore = tokenStore;
        }

        public async Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default)
        {
            return await SendAsync<T>(HttpMethod.Get, relativeUrl, null, ct);
        }

        public async Task<T> PostAsync<T>(string relativeUrl, object data, CancellationToken ct = default)
        {
            return await SendAsync<T>(HttpMethod.Post, relativeUrl, data, ct);
        }

        public async Task<T> PutAsync<T>(string relativeUrl, object data, CancellationToken ct = default)
        {
            return await SendAsync<T>(HttpMethod.Put, relativeUrl, data, ct);
        }

        public async Task DeleteAsync(string relativeUrl, CancellationToken ct = default)
        {
            await SendAsync<object>(HttpMethod.Delete, relativeUrl, null, ct);
        }

        private async Task<T?> SendAsync<T>(HttpMethod method, string relativeUrl, object? data, CancellationToken ct)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(method, relativeUrl);

            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _http.SendAsync(req, ct);

            var body = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{body}");

            if (typeof(T) == typeof(object) && string.IsNullOrWhiteSpace(body))
                return default;

            var resultData = JsonSerializer.Deserialize<T>(body, _jsonOptions);
            if (resultData is null && typeof(T) != typeof(object)) // Allow null for void/object returns if body checks out? Or strict? 
                 // If T is expected but body is null, it might be an error or empty list. 
                 // But JsonSerializer usually returns null only for "null" string.
                 // Let's stick to previous logic but updated.
                 throw new InvalidOperationException("Empty/invalid JSON response.");

            return resultData!;
        }
    }
}
