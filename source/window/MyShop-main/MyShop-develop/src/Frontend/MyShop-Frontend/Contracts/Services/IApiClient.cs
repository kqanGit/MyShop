using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IApiClient
    {
        // GET request
        Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default);

        // GET request for file download (returns byte array)
        Task<byte[]> GetBytesAsync(string relativeUrl, CancellationToken ct = default);

        // POST request (tạo mới)
        Task<T> PostAsync<T>(string relativeUrl, object body, CancellationToken ct = default);

        // PUT request (cập nhật)
        Task<T> PutAsync<T>(string relativeUrl, object body, CancellationToken ct = default);

        // DELETE request
        Task<bool> DeleteAsync(string relativeUrl, CancellationToken ct = default);
    }
}