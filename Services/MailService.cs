using System.Net.Mail;
using System.Net;
using System.Text;
using VAirZoneWebAPI.Interfaces;

namespace VAirZoneWebAPI.Services
{
	public class MailService(IConfiguration config) : IMailService
	{
		public async Task SendForgetPasswordCode(string to, string code)
		{
			var address = config["MailSettings:Address"];
			var sender = config["MailSettings:Sender"];
			var password = config["MailSettings:Password"];
			var host = config["MailSettings:Host"];
			var port = config.GetValue<int>("MailSettings:Port");

			using MailMessage mailMessage = new();
			mailMessage.From = new MailAddress(address, sender, Encoding.UTF8);
			mailMessage.To.Add(new MailAddress(to));
			mailMessage.Subject = "Mã xác thực đổi mật khẩu";
			mailMessage.Body = $@"<b>Mã xác thực: </b>{code}
								<br>
								<i><strong>Mã sẽ hết hạn sau 10 phút</strong></i>";

			mailMessage.SubjectEncoding = Encoding.UTF8;
			mailMessage.BodyEncoding = Encoding.UTF8;
			mailMessage.IsBodyHtml = true;

			using SmtpClient smtpClient = new();
			smtpClient.Host = host;
			smtpClient.Port = port;
			smtpClient.EnableSsl = true;
			smtpClient.Credentials = new NetworkCredential(address, password);

			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}