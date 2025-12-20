using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.DTOs;
using MyShop.Domain.Entities;
using MyShop.Domain.Repositories;
using BCrypt.Net;

namespace MyShop.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(LoginRequestDto request);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository users, IConfiguration configuration)
        {
            _users = users;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto request)
        {
            var user = await _users.GetByUsernameAsync(request.Username);

            bool isActive = user?.Status != null && user.Status.Length > 0 && user.Status[0];

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
                Email = "email-removed-in-db-schema",
                Role = user.RoleId.ToString(),
                ExpiresAt = expiresAt
            };
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