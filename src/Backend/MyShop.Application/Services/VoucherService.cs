using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Application.DTOs.Common;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Services
{

    public interface IVoucherService
    {
        Task<CheckVoucherDto> GetVoucherById(int voucherId);
        Task<CheckVoucherDto> GetVoucherByCode(string voucherCode);
        Task<VoucherDto> CreateVoucher(CreateVoucherDto voucherDto);
    }
    public class VoucherService : IVoucherService
    {
        private readonly AppDbContext _context;
        public VoucherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckVoucherDto> GetVoucherById(int voucherId)
        {
            var voucher = await _context.Vouchers
                .Where(v => v.VoucherId == voucherId)
                .FirstOrDefaultAsync();
            
            if (voucher == null)
            {
                return null;
            }

            var isValid = true;
            var currentDate = DateTime.UtcNow;

            if (voucher.StartDate.HasValue && currentDate < voucher.StartDate.Value)
            {
                isValid = false;
            }
            if (voucher.EndDate.HasValue && currentDate > voucher.EndDate.Value)
            {
                isValid = false;
            }
            if (voucher.IsRemoved == true)
            {
                isValid = false;
            }

            return new CheckVoucherDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCode = voucher.VoucherCode,
                Discount = voucher.Discount,
                MinThreshold = voucher.MinThreshold,
                IsValid = isValid
            };
        }

        public async Task<CheckVoucherDto> GetVoucherByCode(string voucherCode)
        {
            var voucher = await _context.Vouchers
                .Where(v => v.VoucherCode == voucherCode)
                .FirstOrDefaultAsync();

            if (voucher == null)
            {
                return null;
            }

            var isValid = true;
            var currentDate = DateTime.UtcNow;

            if (voucher.StartDate.HasValue && currentDate < voucher.StartDate.Value)
            {
                isValid = false;
            }
            if (voucher.EndDate.HasValue && currentDate > voucher.EndDate.Value)
            {
                isValid = false;
            }
            if (voucher.IsRemoved == true)
            {
                isValid = false;
            }

            return new CheckVoucherDto
            {
                VoucherId = voucher.VoucherId,
                VoucherCode = voucher.VoucherCode,
                Discount = voucher.Discount,
                MinThreshold = voucher.MinThreshold,
                IsValid = isValid
            };
        }

        public async Task<VoucherDto> CreateVoucher(CreateVoucherDto voucherDto)
        {
            var maxId = await _context.Vouchers.MaxAsync(v => (int?)v.VoucherId) ?? 0;
            var newVoucher = new Domain.Entities.Voucher
            {
                VoucherId = maxId + 1,
                VoucherCode = voucherDto.VoucherCode,
                Description = voucherDto.Description,
                Type = voucherDto.Type,
                Discount = voucherDto.Discount,
                MinThreshold = voucherDto.MinThreshold,
                StartDate = voucherDto.StartDate,
                EndDate = voucherDto.EndDate,
                IsRemoved = false
            };
            _context.Vouchers.Add(newVoucher);
            await _context.SaveChangesAsync();
            return new VoucherDto
            {
                VoucherId = newVoucher.VoucherId,
                VoucherCode = newVoucher.VoucherCode,
                Description = newVoucher.Description,
                Type = newVoucher.Type,
                Discount = newVoucher.Discount,
                MinThreshold = newVoucher.MinThreshold,
                StartDate = newVoucher.StartDate,
                EndDate = newVoucher.EndDate,
                IsRemoved = newVoucher.IsRemoved
            };
        }
    }
}
