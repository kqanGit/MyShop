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
        Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default);
        Task<T> PostAsync<T>(string relativeUrl, object data, CancellationToken ct = default);
        Task<T> PutAsync<T>(string relativeUrl, object data, CancellationToken ct = default);
        Task DeleteAsync(string relativeUrl, CancellationToken ct = default);
    }
}
