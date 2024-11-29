namespace VAirZoneWebAPI.Interfaces
{
	public interface IGoogleAIService
	{
		Task<dynamic> GetResult(string text);
	}
}
