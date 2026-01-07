using System.Threading;
using System.Threading.Tasks;
using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;

namespace MyShop_Frontend.Services
{
    public sealed class UserConfigService : IUserConfigService
    {
        private readonly IApiClient _api;

        public UserConfigService(IApiClient api)
        {
            _api = api;
        }

        public Task<UserConfigClientDto?> GetConfigAsync(CancellationToken ct = default)
            => _api.GetAsync<UserConfigClientDto>("api/user-configs", ct);

        public Task<UserConfigClientDto?> SaveConfigAsync(UserConfigClientDto request, CancellationToken ct = default)
            => _api.PutAsync<UserConfigClientDto>("api/user-configs", request, ct);
    }
}
