namespace Ticketing.GraphQL.Web.EmailService;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}