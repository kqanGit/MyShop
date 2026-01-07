using MyShop_Frontend.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IUserService
    {
        /// <summary>
        /// POST /api/users
        /// </summary>
        Task<UserDto> CreateStaffAsync(CreateUserDto request, CancellationToken ct = default);

        /// <summary>
        /// GET /api/users/profile
        /// </summary>
        Task<UserDto> GetProfileAsync(CancellationToken ct = default);
    }
}
