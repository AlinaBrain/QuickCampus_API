using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QuickCampusAPI.JWT_Helper
{
    public class JWTHelper
    {
        public static string GetIdFromToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = token.Replace("Bearer ", string.Empty);
            var key = Encoding.ASCII.GetBytes(secretKey);

            // Set the token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // Validate and decode the JWT token
                var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Retrieve the "id" claim value
                var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Role")?.Value;
                // var idClaim = jwtToken.Claims.SingleOrDefault(c => c.Type == "name")?.Value;

                return nameClaim;
            }
            catch (Exception)
            {
                // Handle any exceptions, such as token validation failure
                return null;
            }
        }
    }
}
