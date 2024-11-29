namespace VAirZoneWebAPI.Interfaces
{
	public interface IMailService
	{
		Task SendForgetPasswordCode(string to, string code);
	}
}