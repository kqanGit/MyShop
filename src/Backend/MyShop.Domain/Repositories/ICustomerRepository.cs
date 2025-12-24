using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByPhoneAsync(string phoneNumber);
        Task<Customer?> GetCustomerWithMembershipAsync(int id);
    }
}
