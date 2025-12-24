using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using BCrypt.Net;

namespace MyShop.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request);
        Task<AuthResponseDto> Register(RegisterRequestDto request);
        Task Logout(string refreshToken);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto request)
        {
            // Note: Status is nullable BitArray? in new schema
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            // Manual check for Status because BitArray translation in SQL is tricky
            // Status[0] represents the first bit.
            bool isActive = user?.Status != null && user.Status.Length > 0 && user.Status[0];

            // Note: Password in DB might not be hashed yet? Assuming BCrypt for now based on previous code.
            // If DB stores plain text, this verify will fail. 
            if (user == null || !isActive || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["JwtSettings:ExpirationMinutes"]));

            return new AuthResponseDto
            {
                Token = token,
                Username = user.UserName,
                Email = "email-removed-in-db-schema", // Placeholder
                Role = user.RoleId.ToString(), // Converted Int to String
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> Register(RegisterRequestDto request)
        {
            // Kiểm tra username đã tồn tại
            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Email check removed as Email field does not exist in new schema
            /*
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }
            */

            var user = new User
            {
                UserName = request.Username,
                // Email field removed
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                // PhoneNumber field removed
                RoleId = 1, // Default Role ID ???
                Status = new BitArray(new[] { true })
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["JwtSettings:ExpirationMinutes"]));

            return new AuthResponseDto
            {
                Token = token,
                Username = user.UserName,
                Email = "email-removed",
                Role = user.RoleId.ToString(),
                ExpiresAt = expiresAt
            };
        }

        public async Task Logout(string refreshToken)
        {
            // In a real app with refresh tokens, we would revoke it here.
            // For now, we just acknowledge the logout request.
            await Task.CompletedTask;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                // Email claim removed or empty
                new Claim(JwtRegisteredClaimNames.Email, "no-email"),
                new Claim(ClaimTypes.Role, user.RoleId.ToString() ?? "0"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JwtSettings:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}