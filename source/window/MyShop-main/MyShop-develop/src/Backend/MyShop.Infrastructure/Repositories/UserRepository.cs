using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public Task<bool> ExistsByUsernameAsync(string username)
        {
            return Set.AnyAsync(u => u.UserName == username);
        }
    }
}
