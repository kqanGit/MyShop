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
    }
}
