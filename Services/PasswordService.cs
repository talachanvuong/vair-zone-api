using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Services
{
	public class PasswordService(IConfiguration config) : IPasswordService
	{
		private readonly string secretKey = config["JWT:SecretKey"];

		public string CreatePassword(string password)
		{
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password + secretKey, 13);
		}

		public bool VerifyPassword(string password, string hash)
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(password + secretKey, hash);
		}
	}
}