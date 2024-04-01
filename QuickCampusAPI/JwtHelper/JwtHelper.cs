using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

            // Retrieve the "Clientid" claim value
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            return nameClaim;
        }
        catch (Exception)
        {
            // Handle any exceptions, such as token validation failure
            return null;
        }
    }


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
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Retrieve the "id" claim value
            var nameClaim = jwtToken.Claims.First(c => c.Type == "UserId").Value;


            return nameClaim;
        }
        catch (Exception)
        {
            // Handle any exceptions, such as token validation failure
            return null;
        }
    }

    public static string GetClientIdFromToken(string token, string secretKey)
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

            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Retrieve the "id" claim value
            string nameClaim = string.Empty;

            nameClaim = jwtToken.Claims.First(c => c.Type == "cilentId").Value;


            return nameClaim;
        }
        catch (Exception)
        {
            // Handle any exceptions, such as token validation failure
            return null;
        }
    
    
  }

    public static string GetuIdFromToken(string token, string secretKey)
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
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Retrieve the "id" claim value
            string nameClaim = string.Empty;

            nameClaim = jwtToken.Claims.First(c => c.Type == "UserId").Value;
            return nameClaim;
        }
        catch (Exception)
        {
            // Handle any exceptions, such as token validation failure
            return null;
        }
    }

    public static string GetCraetedByFromToken(string token, string secretKey)
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
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            // Retrieve the "id" claim value
            string nameClaim = string.Empty;
            nameClaim = jwtToken.Claims.First(c => c.Type == "CreatedBy").Value;
            return nameClaim;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static bool isSuperAdminfromToken(string token, string secretKey)
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
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            // Retrieve the "id" claim value
            string nameClaim = string.Empty;
            nameClaim = jwtToken.Claims.First(c => c.Type == "IsSuperAdmin").Value;
            return nameClaim=="1"?true:false;
        }
        catch (Exception)
        {
            // Handle any exceptions, such as token validation failure
            return false;
        }
    }

}
