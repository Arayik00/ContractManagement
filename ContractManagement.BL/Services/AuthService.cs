using ContractManagement.BL.Interfaces;
using ContractManagement.BL.Entities;
using ContractManagement.BL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _users.GetByCredentialsAsync(username, password);
            if (user == null) return null;
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                new Claim(ClaimTypes.Name, user.FirstName ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
