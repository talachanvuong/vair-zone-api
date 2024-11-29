using Microsoft.Playwright;

namespace VAirZoneWebAPI.Interfaces
{
	public interface ICookieService
	{
		Task GetCookies(IBrowserContext browserContext);
		Task SetCookies(IBrowserContext browserContext);
	}
}
