using MyShop_Frontend.Contracts.Services;
using MyShop_Frontend.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IApiClient _api;

        public MembershipService(IApiClient api)
        {
            _api = api;
        }

        public async Task<IEnumerable<Membership>> GetMembershipsAsync(CancellationToken ct = default)
        {
            return await _api.GetAsync<IEnumerable<Membership>>("api/Membership", ct);
        }
    }
}
