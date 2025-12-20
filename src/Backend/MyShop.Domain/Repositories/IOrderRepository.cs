using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetByStatusAsync(string status);
    }
}
