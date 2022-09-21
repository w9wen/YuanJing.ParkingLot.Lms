using System.Threading.Tasks;

namespace w9wen.dotnet.Template.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body);
}
