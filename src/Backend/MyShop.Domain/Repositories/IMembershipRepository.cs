using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IMembershipRepository : IRepository<Membership>
    {
        Task<Membership?> GetByTierNameAsync(string tierName);
        Task<Membership?> GetTierByPointAsync(int point);
    }
}
