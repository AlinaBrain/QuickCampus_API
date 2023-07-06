using DocumentFormat.OpenXml.InkML;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public class JwtHelper
{
    public static string GetUserIdFromToken(string token, string secretKey)
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
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
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
