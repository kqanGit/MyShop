using System.Threading.Tasks;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
    }
}