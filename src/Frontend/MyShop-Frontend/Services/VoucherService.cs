using MyShop_Frontend.Contracts.Dtos;
using MyShop_Frontend.Contracts.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop_Frontend.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IApiClient _api;

        public VoucherService(IApiClient api)
        {
            _api = api;
        }

        public async Task<VoucherDto> GetVoucherByIdAsync(int voucherId, CancellationToken ct = default)
        {
            return await _api.GetAsync<VoucherDto>($"api/Voucher/{voucherId}", ct);
        }

        public async Task<VoucherCheckResponseDto> CheckVoucherAsync(string voucherCode, CancellationToken ct = default)
        {
            return await _api.GetAsync<VoucherCheckResponseDto>($"api/Voucher/check/{voucherCode}", ct);
        }

        public async Task<VoucherDto> CreateVoucherAsync(CreateVoucherDto request, CancellationToken ct = default)
        {
            return await _api.PostAsync<VoucherDto>("api/Voucher", request, ct);
        }
    }
}
