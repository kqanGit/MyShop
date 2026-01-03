using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IMembershipService
    {
        Task<IEnumerable<Membership>> GetMembershipsAsync(CancellationToken ct = default);
    }
}
