using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await Set.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(int status)
        {
            return await Set.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
          
            return await Set
                .Where(o => o.OrderDate.HasValue && o.OrderDate >= fromDate && o.OrderDate <= toDate)
                .ToListAsync();
        }

        public Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return Set
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.Voucher)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
    }
}
