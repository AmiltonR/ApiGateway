using JwtAuthenticationManager.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "yPkCqn4kSWLtaJwXvN2jGzQRyTZ3gdXkt7FeBJP";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        
        public JwtTokenHandler()
        {
            
        }

        public AuthenticationResponse? GenerateJwtToken(UserAccount user)
        {
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Carnet),
                new Claim("Role", user.Role.ToString())
            }) ;

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var date = DateTime.UtcNow;
            
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials,
                NotBefore = date,
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                Carnet = user.Carnet,
                Rol = user.Role,
                Nombre = user.Nombre,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
                JwtToken = token
            };
        }
    }
}
