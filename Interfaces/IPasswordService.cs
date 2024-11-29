namespace VAirZoneWebAPI.Interfaces
{
	public interface IPasswordService
	{
		string CreatePassword(string password);
		bool VerifyPassword(string password, string hash);
	}
}
