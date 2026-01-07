using System.Threading;
using System.Threading.Tasks;
using MyShop_Frontend.Contracts.Dtos;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IUserConfigService
    {
        Task<UserConfigClientDto?> GetConfigAsync(CancellationToken ct = default);
        Task<UserConfigClientDto?> SaveConfigAsync(UserConfigClientDto request, CancellationToken ct = default);
    }
}
