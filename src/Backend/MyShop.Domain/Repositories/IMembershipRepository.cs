using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task<Membership?> GetActiveByCustomerIdAsync(int customerId);
        Task<IEnumerable<Membership>> GetByTierAsync(string tier);
    }
}
