using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class UserService : IUserService
    {
        private readonly IApiClient _api;

        public UserService(IApiClient api)
        {
            _api = api;
        }

        public async Task<UserDto> CreateStaffAsync(CreateUserDto request, CancellationToken ct = default)
        {
            return await _api.PostAsync<UserDto>("api/users", request, ct);
        }

        public async Task<UserDto> GetProfileAsync(CancellationToken ct = default)
        {
            return await _api.GetAsync<UserDto>("api/users/profile", ct);
        }
    }
}
