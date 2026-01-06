using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class UserConfigRepository : Repository<UserConfig>, IUserConfigRepository
    {
        public UserConfigRepository(AppDbContext context) : base(context)
        {
        }

        public Task<UserConfig?> GetByUserIdAsync(int userId)
        {
            return Set.FirstOrDefaultAsync(uc => uc.UserId == userId);
        }
    }
}
