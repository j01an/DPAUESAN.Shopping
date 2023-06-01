﻿using DPAUESAN.Shopping.DOMAIN.Core.Entities;
using DPAUESAN.Shopping.DOMAIN.Core.Interfaces;
using DPAUESAN.Shopping.DOMAIN.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DPAUESAN.Shopping.DOMAIN.Infrastructure.Shared
{
    public class JWTFactory : IJWTFactory
    {
        public JWTSettings _settings { get; }
        public JWTFactory(IOptions<JWTSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateJWToken(User user)
        {
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(sc);

            var claims = new[] {
                    new Claim(ClaimTypes.Name, (user.FirstName + "" + user.LastName)),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
                    new Claim(ClaimTypes.Country, user.Country),
                    new Claim(ClaimTypes.Role, user.Type == "A" ? "Admin": "User"),
                    new Claim("UserId",user.Id.ToString()),
                };

            var payload = new JwtPayload(
                            _settings.Issuer
                            , _settings.Audience
                            , claims
                            , DateTime.UtcNow
                            , DateTime.UtcNow.AddMinutes(_settings.DurationInMinutes));

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
