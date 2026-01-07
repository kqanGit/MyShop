using System.Threading.Tasks;
using System.Collections.Generic;
using MyShop.Domain.Entities;

namespace MyShop.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<(List<Order>, int)> GetPagedOrdersAsync(int? userId, int pageIndex, int pageSize, DateTime? fromDate, DateTime? toDate);
    }
}
