using MyShop_Frontend.Contracts.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Contracts.Services
{
    public interface IVoucherService
    {
        /// <summary>
        /// GET /api/Voucher/{voucherId}
        /// </summary>
        Task<VoucherDto> GetVoucherByIdAsync(int voucherId, CancellationToken ct = default);

        /// <summary>
        /// GET /api/Voucher/check/{voucherCode}
        /// </summary>
        Task<VoucherCheckResponseDto> CheckVoucherAsync(string voucherCode, CancellationToken ct = default);

        /// <summary>
        /// POST /api/Voucher
        /// </summary>
        Task<VoucherDto> CreateVoucherAsync(CreateVoucherDto request, CancellationToken ct = default);
    }
}
