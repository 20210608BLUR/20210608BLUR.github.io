using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HolyShong.BackStage.Helpers
{
    public class JwtHelper
    {
        public static string Issuer => "HolyShong";
        public static SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hauehragjegaffhlkjeqhoeury"));
        
        public string GenerateToken(string username)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var signCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            //payload
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = Issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = userClaimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signCredentials
            };

            //完成token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);
            return serializeToken;
        }
    }
}
