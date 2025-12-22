using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;

namespace MyShop.Infrastructure.Repositories
{
    public class VoucherRepository : Repository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Voucher?> GetByCodeAsync(string code)
        {
            return Set.FirstOrDefaultAsync(v => v.VoucherCode == code);
        }

        public async Task<IEnumerable<Voucher>> GetActiveAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await Set.Where(v => v.IsRemoved == false && v.StartDate.HasValue && v.EndDate.HasValue && v.StartDate <= today && v.EndDate >= today).ToListAsync();
        }
    }
}
