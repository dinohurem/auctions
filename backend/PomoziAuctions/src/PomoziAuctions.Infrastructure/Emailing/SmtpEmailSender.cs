using System.Net.Mail;
using System.Text;
using PomoziAuctions.Core.Abstractions;

namespace PomoziAuctions.Infrastructure.Emailing;

public class SmtpEmailSender : IEmailSender
{
	public async Task SendAsync(string from, string to, string subject, string body)
	{
		var client = new SmtpClient("localhost", 25)
		{
			DeliveryMethod = SmtpDeliveryMethod.Network,
		};

		await client.SendMailAsync(new MailMessage(from, to, subject, body)
		{
			IsBodyHtml = true,
			BodyEncoding = Encoding.UTF8,
			SubjectEncoding = Encoding.UTF8,
			HeadersEncoding = Encoding.UTF8,
		});
	}

	public Task SendAsync(string from, string to, string templateId, object data)
	{
		throw new NotImplementedException();
	}
}
