using MyShop_Frontend.Contracts.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

        // GET request
        public async Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(HttpMethod.Get, relativeUrl);

            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _http.SendAsync(req, ct);

            var body = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{body}");

            var data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
            if (data is null)
                throw new InvalidOperationException("Empty/invalid JSON response.");

            return data;
        }

        // GET request for file download (returns byte array)
        public async Task<byte[]> GetBytesAsync(string relativeUrl, CancellationToken ct = default)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(HttpMethod.Get, url);

            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _http.SendAsync(req, ct);

            if (!res.IsSuccessStatusCode)
            {
                var errorBody = await res.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{errorBody}");
            }

            return await res.Content.ReadAsByteArrayAsync(ct);
        }

        // POST request (tạo mới)
        public async Task<T> PostAsync<T>(string relativeUrl, object body, CancellationToken ct = default)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(HttpMethod.Post, url);

            // Add JWT token
            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serialize body to JSON
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var res = await _http.SendAsync(req, ct);
            var responseBody = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{responseBody}");

            var data = JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            if (data is null)
                throw new InvalidOperationException("Empty/invalid JSON response.");

            return data;
        }

        // PUT request (cập nhật)
        public async Task<T> PutAsync<T>(string relativeUrl, object body, CancellationToken ct = default)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(HttpMethod.Put, url);

            // Add JWT token
            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serialize body to JSON
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var res = await _http.SendAsync(req, ct);
            var responseBody = await res.Content.ReadAsStringAsync(ct);

            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{responseBody}");

            var data = JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            if (data is null)
                throw new InvalidOperationException("Empty/invalid JSON response.");

            return data;
        }

        // DELETE request
        public async Task<bool> DeleteAsync(string relativeUrl, CancellationToken ct = default)
        {
            var url = relativeUrl.TrimStart('/');

            using var req = new HttpRequestMessage(HttpMethod.Delete, url);

            // Add JWT token
            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _http.SendAsync(req, ct);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"HTTP {(int)res.StatusCode} - {res.ReasonPhrase}\n{body}");
            }

            return res.IsSuccessStatusCode;
        }
    }
}
