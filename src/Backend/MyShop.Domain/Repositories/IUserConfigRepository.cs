using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IUserConfigRepository : IRepository<UserConfig>
    {
        // Add specific queries here when needed, e.g. by user id
        Task<UserConfig?> GetByUserIdAsync(int userId);
    }
}
