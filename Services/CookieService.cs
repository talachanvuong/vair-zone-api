using Microsoft.Playwright;
using Newtonsoft.Json;
using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Services
{
	public class CookieService : ICookieService
	{
		private const string FILE_PATH = "cookies.json";

		public async Task GetCookies(IBrowserContext browserContext)
		{
			if (!File.Exists(FILE_PATH))
			{
				Console.WriteLine("Cannot get cookies!");
			}

			var cookies = await browserContext.CookiesAsync();
			File.WriteAllText(FILE_PATH, JsonConvert.SerializeObject(cookies, Formatting.Indented));
		}

		public async Task SetCookies(IBrowserContext browserContext)
		{
			if (!File.Exists(FILE_PATH))
			{
				Console.WriteLine("Cannot set cookies!");
			}

			var cookiesJson = File.ReadAllText(FILE_PATH);
			var cookies = JsonConvert.DeserializeObject<Cookie[]>(cookiesJson);
			await browserContext.AddCookiesAsync(cookies);
		}
	}
}
