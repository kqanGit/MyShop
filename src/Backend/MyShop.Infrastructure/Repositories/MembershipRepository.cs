using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        public MembershipRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Membership?> GetByTierNameAsync(string tierName)
        {
            return Set.FirstOrDefaultAsync(m => m.TierName == tierName);
        }

        public Task<Membership?> GetTierByPointAsync(int point)
        {
            return Set.FirstOrDefaultAsync(m => point >= m.Threshold);
        }
    }
}
