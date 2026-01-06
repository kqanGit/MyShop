using System.Collections;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;

namespace MyShop.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> CreateStaff(CreateUserDto request);
        Task<UserDto> GetProfile(ClaimsPrincipal userClaims);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> CreateStaff(CreateUserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
                throw new InvalidOperationException("Username already exists");

            var user = new User
            {
                UserName = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                RoleId = request.RoleId,
                Status = new BitArray(new[] { true }),
                PhoneNumber = request.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.UserName,
                FullName = user.FullName,
                RoleId = user.RoleId ?? 0,
                IsActive = true
            };
        }

        public async Task<UserDto> GetProfile(ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier) ?? userClaims.FindFirst("sub");
            if (userIdClaim == null) throw new UnauthorizedAccessException();

            int userId = int.Parse(userIdClaim.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) throw new KeyNotFoundException("User not found");

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.UserName,
                FullName = user.FullName,
                RoleId = user.RoleId ?? 0,
                IsActive = user.Status != null && user.Status.Length > 0 && user.Status[0]
            };
        }
    }
}
