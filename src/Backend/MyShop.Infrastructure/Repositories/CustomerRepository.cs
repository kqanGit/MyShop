using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Customer?> GetByPhoneAsync(string phoneNumber)
        {
            return Set.FirstOrDefaultAsync(c => c.Phone == phoneNumber);
        }

        public Task<Customer?> GetCustomerWithMembershipAsync(int id)
        {
            return Set.Include(c => c.Tier).FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
