namespace VAirZoneWebAPI.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(string email, string password);
	}
}
