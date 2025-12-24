using MyShop.Application.DTOs;
using MyShop.Application.DTOs.Common;
using MyShop.Application.DTOs.Order;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyShop.Application.Services
{   
    public interface IOrderService
    {
        Task<OrderResultDto> CheckoutAsync(CreateOrderRequest request, int userId);

        Task<PagedResult<OrderSummaryDto>> GetMyOrdersAsync(GetOrdersRequest request);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMembershipRepository _membershipRepo;
        private readonly IOrderRepository _orderRepo;

        public OrderService(
            AppDbContext context,
            IProductRepository productRepo,
            IVoucherRepository voucherRepo,
            ICustomerRepository customerRepo,
            IMembershipRepository membershipRepo,
            IOrderRepository orderRepo)
        {
            _context = context;
            _productRepo = productRepo;
            _voucherRepo = voucherRepo;
            _customerRepo = customerRepo;
            _membershipRepo = membershipRepo;
            _orderRepo = orderRepo;
        }

        public async Task<OrderResultDto> CheckoutAsync(CreateOrderRequest request, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Create order (CustomerId can be null)
                var order = new Order
                {
                    OrderCode = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                    OrderDate = DateTime.UtcNow,
                    UserId = userId, // ID of salesperson or the logged-in user
                    CustomerId = request.CustomerId, 
                    Status = 1
                };

                decimal totalPrice = 0;
                var orderDetails = new List<OrderDetail>();

                // 2. Iterate products to calculate totals and decrement stock
                foreach (var item in request.Items)
                {
                    var product = await _productRepo.GetByIdAsync(item.ProductId);

                    if (product == null) throw new Exception($"Product with ID {item.ProductId} does not exist.");

                    if (product.IsRemoved[0]) throw new Exception($"Product {product.ProductName} has been discontinued.");

                    if (product.Stock < item.Quantity)
                        throw new Exception($"Product {product.ProductName} is out of stock (Remaining: {product.Stock}).");

                    // Decrement stock
                    product.Stock -= item.Quantity;
                    await _productRepo.UpdateAsync(product);

                    // Save order detail (price snapshot at purchase time)
                    var detail = new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = item.Quantity,
                        CurrentPrice = product.Price,
                        CurrentCost = product.Cost,
                        TotalLine = product.Price * item.Quantity
                    };

                    orderDetails.Add(detail);
                    totalPrice += detail.TotalLine;
                }

                order.OrderDetails = orderDetails;
                order.TotalPrice = totalPrice;

                // 3. Customer handling (only if CustomerId provided)
                Customer? customer = null;
                decimal totalDiscount = 0;

                if (request.CustomerId != 1) // Only run when customer ID is provided
                {
                    // Load customer and membership tier
                    customer = await _customerRepo.GetCustomerWithMembershipAsync(request.CustomerId);

                    // Apply membership discount
                    if (customer != null && customer.Tier != null)
                    {
                        decimal discountPercent = (decimal)(customer.Tier.Discount ?? 0);
                        decimal memberDiscount = totalPrice * (discountPercent / 100);
                        totalDiscount += memberDiscount;
                    }
                }

                // 4. Voucher handling (anyone can use with a valid code)
                if (!string.IsNullOrEmpty(request.VoucherCode))
                {
                    var voucher = await _voucherRepo.GetByCodeAsync(request.VoucherCode);

                    if (voucher == null) throw new Exception("Voucher does not exist.");

                    if (voucher.IsRemoved == true)
                        throw new Exception("Voucher has been removed.");

                    if (voucher.StartDate > DateTime.UtcNow || voucher.EndDate < DateTime.UtcNow)
                        throw new Exception("Voucher has not started or has expired.");

                    if (totalPrice < voucher.MinThreshold)
                        throw new Exception($"Order must be at least {voucher.MinThreshold:N0} VND to use this voucher.");

                    decimal voucherDiscount = 0;
                    if (voucher.Type == 2) // Fixed amount discount
                    {
                        voucherDiscount = voucher.Discount;
                    }
                    else // Percentage discount
                    {
                        voucherDiscount = totalPrice * (voucher.Discount / 100m);
                    }

                    totalDiscount += voucherDiscount;
                    order.VoucherId = voucher.VoucherId;
                }

                // 5. Finalize totals
                order.DiscountAmount = totalDiscount;
                order.FinalPrice = totalPrice - totalDiscount;
                if (order.FinalPrice < 0) order.FinalPrice = 0;

                // 6. Save to database
                await _orderRepo.AddAsync(order);
                await _context.SaveChangesAsync(); // OrderId is generated after this

                // 7. Reward points & tier upgrade (only for registered customers)
                int pointsEarned = 0;
                if (request.CustomerId != 1) // Double-check customer exists
                {
                    pointsEarned = (int)(order.FinalPrice / 1000); // 1000 VND = 1 point

                    // Increase points (null-safe)
                    customer.Point = customer.Point + pointsEarned;

                    // Check tier upgrade
                    // Get the highest tier matching current points
                    var newTier = await _membershipRepo.GetTierByPointAsync(customer.Point);

                    if (newTier != null && newTier.TierId != customer.TierId)
                    {
                        customer.TierId = newTier.TierId;
                    }

                    await _customerRepo.UpdateAsync(customer);
                }

                // 8. Complete transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OrderResultDto
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    FinalPrice = order.FinalPrice,
                    EarnedPoints = pointsEarned, // Walk-in customers will have 0
                    Message = "Checkout successful!"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // On error, rollback everything
                throw;
            }
        }

         public async Task<PagedResult<OrderSummaryDto>> GetMyOrdersAsync(GetOrdersRequest request)
        {
            // Call repository with correct date range
            var (orders, totalCount) = await _orderRepo.GetPagedOrdersAsync(
                request.PageIndex,
                request.PageSize,
                request.FromDate,
                request.ToDate
            );

            // Map to lightweight DTOs
            var dtos = orders.Select(o => new OrderSummaryDto
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                OrderDate = o.OrderDate,
                FinalPrice = o.FinalPrice,
                StatusName = o.Status switch { 1 => "New", 2 => "Delivering", 3 => "Completed", _ => "Other" },
                TotalItems = o.OrderDetails.Count
            }).ToList();

            return new PagedResult<OrderSummaryDto>(dtos, totalCount, request.PageIndex, request.PageSize);
        }
    }
}