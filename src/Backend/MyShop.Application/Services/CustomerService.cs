using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;

namespace MyShop.Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> SearchCustomers(string phone, string name);
        Task<CustomerDetailDto> GetCustomerDetail(int id);
        Task<CustomerDto> CreateCustomer(CreateCustomerDto request);
        Task<CustomerDto> UpdateCustomer(int id, UpdateCustomerDto request);
    }

    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomers(string phone, string name)
        {
            var query = _context.Customers.Include(c => c.Tier).AsQueryable();

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(c => c.Phone.Contains(phone));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.FullName.Contains(name));

            return await query.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                Phone = c.Phone,
                Address = c.Address,
                Point = c.Point,
                TierName = c.Tier != null ? c.Tier.TierName : "N/A"
            }).ToListAsync();
        }

        public async Task<CustomerDetailDto> GetCustomerDetail(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Tier)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) throw new KeyNotFoundException("Customer not found");

            return new CustomerDetailDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                Point = customer.Point,
                TierName = customer.Tier?.TierName ?? "N/A",
                CreateDate = customer.CreateDate ?? DateTime.MinValue,
                Membership = customer.Tier != null ? new MembershipDto
                {
                    TierId = customer.Tier.TierId,
                    TierName = customer.Tier.TierName,
                    Discount = customer.Tier.Discount ?? 0
                } : null
            };
        }

        public async Task<CustomerDto> CreateCustomer(CreateCustomerDto request)
        {
            // Find the lowest tier (tier with threshold 0 or lowest threshold)
            var defaultTier = await _context.Memberships
                .OrderBy(m => m.Threshold)
                .FirstOrDefaultAsync();

            var customer = new Customer
            {
                FullName = request.FullName,
                Phone = request.Phone,
                Address = request.Address,
                Point = 0,
                TierId = defaultTier?.TierId,
                CreateDate = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                Point = 0,
                TierName = defaultTier?.TierName ?? "N/A"
            };
        }

        public async Task<CustomerDto> UpdateCustomer(int id, UpdateCustomerDto request)
        {
            var customer = await _context.Customers.Include(c => c.Tier).FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null) throw new KeyNotFoundException("Customer not found");

            customer.FullName = request.FullName;
            customer.Phone = request.Phone;
            customer.Address = request.Address;

            await _context.SaveChangesAsync();

            return new CustomerDto
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                Point = customer.Point,
                TierName = customer.Tier?.TierName ?? "N/A"
            };
        }
    }
}
