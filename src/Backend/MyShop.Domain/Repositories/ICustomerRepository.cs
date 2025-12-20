using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<IEnumerable<Customer>> GetByPhoneAsync(string phoneNumber);
    }
}
