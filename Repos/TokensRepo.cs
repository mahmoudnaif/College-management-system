using College_managemnt_system.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace College_managemnt_system.Repos
{
    public class TokensRepo : ITokensRepo
    {
        public  string generateLoginJWT(int id, string role, string key)
        {
            var claims = new Claim[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim("role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddHours(3),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public  string generateVerficationJWT(int id, string key)
        {
            var claims = new Claim[]
            {
            new Claim("accountId", id.ToString()),
            new Claim("Verify", "true"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddMinutes(10),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string generateChangePasswordJWT(int id, string key)
        {
            var claims = new Claim[]
            {
            new Claim("accountId", id.ToString()),
            new Claim("changepassword", "true"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,
              ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
              ClaimValueTypes.Integer64)
            };

            var keyInBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyInBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
           issuer: "https://localhost:7180",
           audience: "https://localhost:7180",
           claims: claims,
           expires: DateTime.Now.AddMinutes(10),
           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<int> IsTokenValid(string token)
        {
            throw new NotImplementedException();
        }
    }
}
