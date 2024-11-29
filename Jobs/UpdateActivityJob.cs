using Microsoft.Playwright;
using Quartz;
using System.Text.RegularExpressions;
using VAirZoneWebAPI.Interfaces;
using VAirZoneWebAPI.Models.Organizer;

namespace VAirZoneWebAPI.Jobs
{
	[DisallowConcurrentExecution]
	public class UpdateActivityJob(
		IOrganizerRepository organizerRepo,
		IGoogleAIService googleAIService,
		ICookieService cookieService
		) : IJob
	{
		private bool IsActivity(string text)
		{
			string[] agreeAtlas = { "Ngày rèn luyện", "Ngày Rèn Luyện", "NRL", "ngày rèn luyện", "NGÀY RÈN LUYỆN" };
			string[] disagreeAtlas = { "CÔNG BỐ", "TỔNG KẾT", "DANH SÁCH ĐĂNG KÝ THÀNH CÔNG", "RECAP" };

			if (disagreeAtlas.Any(disagree => text.Contains(disagree)))
			{
				return false;
			}

			return agreeAtlas.Any(agree => text.Contains(agree));
		}

		public async Task Execute(IJobExecutionContext context)
		{
			// Create browser
			var playwright = await Playwright.CreateAsync();
			var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
			{
				Headless = true,
				ExecutablePath = "C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe",
				Args = [
					"--disable-gpu",
					"--disable-software-rasterizer",
					"--disable-extensions",
					"--disable-plugins",
					"--disable-blink-features=CSSAnimations,CSSOMViewSmoothScroll",
					"--no-sandbox",
					"--disable-dev-shm-usage",
					"--disable-background-timer-throttling"
				]
			});

			// Create context
			var browserContext = await browser.NewContextAsync(new BrowserNewContextOptions
			{
				ViewportSize = new ViewportSize
				{
					Width = 800,
					Height = 600
				}
			});

			// Set cookies
			await cookieService.SetCookies(browserContext);

			// Open new tab
			var page = await browserContext.NewPageAsync();

			// Get organizers list
			var organizers = await organizerRepo.GetOrganizers();

			foreach (OrganizerModel organizer in organizers)
			{
				await page.GotoAsync(organizer.PageUrl);
				await Task.Delay(1000);

				// Get page content
				string content = await page.ContentAsync();

				// Get organizer page id
				string pageId = organizer.PageUrl[40..];

				try
				{
					// Get post text
					int textTrimIndex = Regex.Match(content, "\"post_id\":.*?\"cix_screen\".*?\"text\":\"").ToString().Length;
					string rawText = Regex.Match(content, "\"post_id\":.*?\"cix_screen\".*?\"text\":\".*?\"\\}").ToString()[textTrimIndex..^2];
					string text = Regex.Unescape(rawText);

					// Check if that post cannot get NRL
					if (!IsActivity(text))
					{
						continue;
					}

					// Get PostUrl
					int postIdTrimIndex = Regex.Match(content, "\"post_id\":\"").ToString().Length;
					string postId = Regex.Match(content, "\"post_id\":\"\\d+\"").ToString()[postIdTrimIndex..^1];
					string postUrl = "https://www.facebook.com/" + pageId + "/posts/" + postId;

					// Check duplicate post
					var activities = await organizerRepo.GetActivitiesById(organizer.OrganizerId);

					if (activities.Any(activity => activity.PostUrl == postUrl))
					{
						continue;
					}

					// Get ImageUrl
					int rawUrlTrimIndex = Regex.Match(content, "\"attachments\":.*?style_list.*?\"uri\":\"").ToString().Length;
					string rawUrl = Regex.Match(content, "\"attachments\":.*?style_list.*?\"uri\":\".*?\"").ToString()[rawUrlTrimIndex..^1];
					string imageUrl = rawUrl.Replace("\\/", "/");

					// Get CreatedOn
					int epochTextTrimIndex = Regex.Match(content, "\"creation_time\":").ToString().Length;
					string epochText = Regex.Match(content, "\"creation_time\":\\d+").ToString()[epochTextTrimIndex..];
					DateTime createdOn = DateTime.UnixEpoch.AddSeconds(double.Parse(epochText)).AddHours(7);

					// Get result from Google AI (Gemini 1.5 Flash)
					var resultAI = await googleAIService.GetResult(text);

					// Create activity
					var activity = new ActivityModel
					{
						ActivityName = resultAI.name,
						Description = resultAI.content,
						PostUrl = postUrl,
						ImageUrl = imageUrl,
						CreatedOn = createdOn,
						OrganizerId = organizer.OrganizerId
					};

					await organizerRepo.CreateAsync(activity);
				}
				catch (Exception e)
				{
					var lineError = int.Parse(e.StackTrace[e.StackTrace.LastIndexOf(' ')..]);

					var error = new ErrorCatcherModel()
					{
						OrganizerId = organizer.OrganizerId,
						Problem = $"{e.Message} {lineError}",
						CreatedOn = DateTime.Now
					};

					await organizerRepo.CreateAsync(error);
				}
			}

			// Retrieve new cookies
			await page.GotoAsync("https://www.facebook.com");
			await Task.Delay(1000);
			await cookieService.GetCookies(browserContext);

			// Close page
			await page.CloseAsync();

			// Exit browser
			await browserContext.CloseAsync();
			await browser.CloseAsync();
			await browserContext.DisposeAsync();
			await browser.DisposeAsync();
			playwright.Dispose();
		}
	}
}