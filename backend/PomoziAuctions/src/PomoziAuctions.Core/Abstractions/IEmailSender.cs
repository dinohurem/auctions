namespace PomoziAuctions.Core.Abstractions;

public interface IEmailSender
{
  Task SendAsync(string from, string to, string subject, string body);

  Task SendAsync(string from, string to, string templateId, object data);
}
