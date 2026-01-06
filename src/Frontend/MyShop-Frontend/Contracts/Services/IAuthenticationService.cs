using MyShop_Frontend.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// POST /api/auth/login
        /// </summary>
        Task<AuthResponseDto?> LoginAsync(string username, string password, CancellationToken ct = default);

        /// <summary>
        /// POST /api/auth/register
        /// </summary>
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default);

        /// <summary>
        /// POST /api/auth/logout
        /// </summary>
        Task<bool> LogoutAsync(CancellationToken ct = default);
    }
}
