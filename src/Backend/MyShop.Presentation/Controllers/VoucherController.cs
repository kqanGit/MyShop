using Microsoft.AspNetCore.Mvc;
using MyShop.Application.DTOs;
using MyShop.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("{voucherId}")]
        public async Task<IActionResult> GetVoucherById(int voucherId)
        {
            try
            {
                var voucher = await _voucherService.GetVoucherById(voucherId);
                if (voucher == null)
                {
                    return NotFound();
                }
                return Ok(voucher);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check/{voucherCode}")]
        public async Task<IActionResult> GetVoucherByCode(string voucherCode)
        {
            try
            {
                var voucher = await _voucherService.GetVoucherByCode(voucherCode);
                if (voucher == null)
                {
                    return NotFound();
                }
                return Ok(voucher);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherDto voucherDto)
        {
            var createdVoucher = await _voucherService.CreateVoucher(voucherDto);
            return Ok(createdVoucher);
        }
    }

}