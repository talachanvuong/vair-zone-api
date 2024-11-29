using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Services
{
	public class TokenService(IConfiguration config) : ITokenService
	{
		public string CreateToken(string email, string password)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Email, email)
			};

			var descriptor = new SecurityTokenDescriptor
			{
				Issuer = config["JWT:Issuer"],
				Subject = new ClaimsIdentity(claims),
				Audience = config["JWT:Audience"],
				Expires = DateTime.Now.AddDays(30),
				NotBefore = DateTime.Now,
				IssuedAt = DateTime.Now,
				SigningCredentials = credentials
			};

			var token = new JwtSecurityTokenHandler().CreateEncodedJwt(descriptor);
			return token;
		}
	}
}