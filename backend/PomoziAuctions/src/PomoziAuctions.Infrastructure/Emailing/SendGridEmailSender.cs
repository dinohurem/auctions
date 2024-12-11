using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using PomoziAuctions.Core.Abstractions;

namespace PomoziAuctions.Infrastructure.Emailing;

public class SendGridEmailSender : IEmailSender
{
	private readonly IOptions<SendGridOptions> _sendGridOptions;

	public SendGridEmailSender(IOptions<SendGridOptions> sendGridOptions)
	{
		_sendGridOptions = sendGridOptions;
	}

	public async Task SendAsync(string from, string to, string subject, string body)
	{
		var client = new SendGridClient(_sendGridOptions.Value.ApiKey);
		var msg = MailHelper.CreateSingleEmail(new EmailAddress(from), new EmailAddress(to), subject, body, body);
		await client.SendEmailAsync(msg).ConfigureAwait(false);
	}

	public async Task SendAsync(string from, string to, string templateId, object data)
	{
		var client = new SendGridClient(_sendGridOptions.Value.ApiKey);
		var msg = MailHelper.CreateSingleTemplateEmail(new EmailAddress(from), new EmailAddress(to), templateId, data);
		await client.SendEmailAsync(msg).ConfigureAwait(false);
	}
}
