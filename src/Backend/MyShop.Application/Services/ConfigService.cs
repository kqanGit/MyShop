using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;

namespace MyShop.Application.Services
{
    public interface IConfigService
    {
        Task<UserConfigDto> GetConfig(ClaimsPrincipal userClaims);
        Task<UserConfigDto> SaveConfig(ClaimsPrincipal userClaims, UserConfigDto request);
    }

    public class ConfigService : IConfigService
    {
        private readonly AppDbContext _context;

        public ConfigService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserConfigDto> GetConfig(ClaimsPrincipal userClaims)
        {
            int userId = GetUserId(userClaims);
            var config = await _context.UserConfigs.FirstOrDefaultAsync(c => c.UserId == userId);

            if (config == null)
            {
                return new UserConfigDto { PerPage = 10, LastModule = "Dashboard" };
            }

            return new UserConfigDto
            {
                PerPage = config.PerPage ?? 10,
                LastModule = config.LastModule
            };
        }

        public async Task<UserConfigDto> SaveConfig(ClaimsPrincipal userClaims, UserConfigDto request)
        {
            int userId = GetUserId(userClaims);
            var config = await _context.UserConfigs.FirstOrDefaultAsync(c => c.UserId == userId);

            if (config == null)
            {
                config = new UserConfig
                {
                    UserId = userId,
                    PerPage = request.PerPage,
                    LastModule = request.LastModule
                };
                _context.UserConfigs.Add(config);
            }
            else
            {
                config.PerPage = request.PerPage;
                config.LastModule = request.LastModule;
            }

            await _context.SaveChangesAsync();

            return new UserConfigDto
            {
                PerPage = config.PerPage ?? 10,
                LastModule = config.LastModule
            };
        }

        private int GetUserId(ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier) ?? userClaims.FindFirst("sub");
            if (userIdClaim == null) throw new UnauthorizedAccessException();
            return int.Parse(userIdClaim.Value);
        }
    }
}
