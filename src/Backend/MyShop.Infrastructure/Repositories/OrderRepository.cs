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

        public async Task<(List<Order>, int)> GetPagedOrdersAsync(int? userId, int pageIndex, int pageSize, DateTime? fromDate, DateTime? toDate)
        {
            // Use repository DbSet as query source
            var query = Set.AsNoTracking().AsQueryable();

            // 1. Filter by user (if provided)
            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId.Value);
            }

            // 2. Filter by date range
            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                // Inclusive end-of-day: compare < next day
                var nextDay = toDate.Value.AddDays(1).Date;
                query = query.Where(o => o.OrderDate < nextDay);
            }

            // 3. Sort newest first
            query = query.OrderByDescending(o => o.OrderDate);

            // 4. Count & paginate
            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
