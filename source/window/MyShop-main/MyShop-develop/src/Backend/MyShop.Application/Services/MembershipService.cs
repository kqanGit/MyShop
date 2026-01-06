using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Services
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipDto>> GetAllMemberships();
        Task<MembershipDto> GetMembershipById(int membershipId);
    }
    public class MembershipService : IMembershipService
    {
        private readonly AppDbContext _context;

        public MembershipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MembershipDto>> GetAllMemberships()
        {
            return await _context.Memberships
                .Select(m => new MembershipDto
                {
                    TierId = m.TierId,
                    TierName = m.TierName,
                    Threshold = m.Threshold,
                    Discount = m.Discount,
                })
                .ToListAsync();
        }

        public async Task<MembershipDto> GetMembershipById(int membershipId)
        {
            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.TierId == membershipId);
            if (membership == null)
            {
                throw new KeyNotFoundException("Membership tier not found.");
            }
            return new MembershipDto
            {
                TierId = membership.TierId,
                TierName = membership.TierName,
                Threshold = membership.Threshold,
                Discount = membership.Discount,
            };
        }
    }
}
