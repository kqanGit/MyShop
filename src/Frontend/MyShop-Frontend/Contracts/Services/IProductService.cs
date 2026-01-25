using MyShop_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken ct = default);
        Task<Product?> GetProductByIdAsync(int productId, CancellationToken ct = default);
    }
}
