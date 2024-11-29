using Newtonsoft.Json;
using System.Text;
using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Services
{
	public class GoogleAIService(IConfiguration config) : IGoogleAIService
	{
		public async Task<dynamic> GetResult(string text)
		{
			string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={config["AI:Key"]}";

			var data = new
			{
				contents = new[]
				{
					new
					{
						role = "user",
						parts = new[]
						{
							new { text }
						}
					}
				},
				systemInstruction = new
				{
					role = "user",
					parts = new[]
					{
						new
						{
							text = "name: Tên tiêu đề, uppercase, nếu có từ \"CHƯƠNG TRÌNH\" ở đầu thì lược bỏ\n" +
								   "content: Viết lại nội dung bằng tiếng việt, không hiển thị icon, emoji, ..., " +
								   "khích lệ người đọc tham dự, làm cho người đọc khoái chí, hoa mĩ (max three sentences)\n\n" +
								   "Người đọc là sinh viên Học viện Hàng không Việt Nam, bạn đóng vai trò là người tổ chức chương trình"
						}
					}
				},
				generationConfig = new
				{
					temperature = 1,
					topP = 0.95,
					maxOutputTokens = 8192,
					responseMimeType = "application/json",
					responseSchema = new
					{
						type = "object",
						properties = new
						{
							name = new { type = "string" },
							content = new { type = "string" }
						},
						required = new[] { "name", "content" }
					}
				}
			};

			var jsonData = JsonConvert.SerializeObject(data);

			using var client = new HttpClient();
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(url, content);

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			dynamic jsonObject = JsonConvert.DeserializeObject(responseContent);
			string rawText = jsonObject.candidates[0].content.parts[0].text;
			dynamic result = JsonConvert.DeserializeObject(rawText);
			return result;
		}
	}
}
