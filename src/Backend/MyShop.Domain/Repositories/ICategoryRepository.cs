using System.Collections.Generic;
using System.Threading.Tasks;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetChildrenAsync(int parentCategoryId);
    }
}
