using Microsoft.AspNetCore.Mvc;
using MyShop.Application.DTOs;
using MyShop.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;
        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMembershipTiers()
        {
            var tiers = await _membershipService.GetAllMemberships();
            return Ok(tiers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipTierById(int id)
        {
            try
            {
                var tier = await _membershipService.GetMembershipById(id);
                return Ok(tier);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}