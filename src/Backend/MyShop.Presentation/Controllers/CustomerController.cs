using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.DTOs;
using MyShop.Application.Services;

namespace MyShop.Presentation.Controllers
{
    [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _customerService.GetCustomers(pageIndex, pageSize);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string? phone = null, [FromQuery] string? name = null, [FromQuery] int? tierId = null)
        {
            if (string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(name) && tierId == null)
            {
               // Allow empty?
            }

            var response = await _customerService.SearchCustomers(pageIndex, pageSize, phone, name, tierId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var response = await _customerService.GetCustomerDetail(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto request)
        {
            var response = await _customerService.CreateCustomer(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto request)
        {
            try
            {
                var response = await _customerService.UpdateCustomer(id, request);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _customerService.DeleteCustomer(id);
            if (!success) return NotFound(new { message = "Customer not found" });
            return NoContent();
        }
    }
}
