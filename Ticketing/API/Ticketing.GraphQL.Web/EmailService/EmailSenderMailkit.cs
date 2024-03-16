using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Ticketing.GraphQL.Web.EmailService;

public class EmailSenderMailkit : IEmailSender
{
    private string MyEmail { get; }
    private string MyPassword { get; }
    private string MySmtpServerHost { get; }
    private int MySmtpPort { get; }
    
    public EmailSenderMailkit()
    {
        MyEmail = Environment.GetEnvironmentVariable("TicketingDDD_EMAIL_MyEmail_Localhost")
                  ?? throw new Exception("TicketingDDD_EMAIL_MyEmail_Localhost nicht im Environment definiert.");
        MyPassword = Environment.GetEnvironmentVariable("TicketingDDD_EMAIL_MyPassword_Localhost")
                     ?? throw new Exception("TicketingDDD_EMAIL_MyPassword_Localhost nicht im Environment definiert.");
        MySmtpServerHost = Environment.GetEnvironmentVariable("TicketingDDD_EMAIL_MySmtpServerHost_Localhost")
                       ?? throw new Exception("TicketingDDD_EMAIL_MySmtpServerHost_Localhost nicht im Environment definiert.");
        MySmtpPort = int.Parse(Environment.GetEnvironmentVariable("TicketingDDD_EMAIL_MySmtpPort_Localhost")
                               ?? throw new Exception("TicketingDDD_EMAIL_MySmtpPort_Localhost nicht im Environment definiert."));
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using (SmtpClient client = new SmtpClient())
        {
            await client.ConnectAsync(MySmtpServerHost, MySmtpPort, true);
            await client.AuthenticateAsync(MyEmail, MyPassword);
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MyEmail, MyEmail));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            await client.SendAsync(message);
            
            Console.WriteLine("Email wird gesendet...");
            await client.DisconnectAsync(true);
        }
    }
    
    
    //
    // public async Task SendEmailAsync(string toEmail, string subject)
    // {
    //     // Set up SMTP client
    //     SmtpClient client = new SmtpClient(MySmtpServerHost, MySmtpPort); // ist das der richtige port für smtps?
    //     client.EnableSsl = true;
    //     // client.Credentials = new NetworkCredential()
    //     client.UseDefaultCredentials = false;
    //     client.Credentials = new NetworkCredential(MyEmail, MyPassword);
    //
    //     // Create email message
    //     MailMessage mailMessage = new MailMessage();
    //     mailMessage.From = new MailAddress(MyEmail);
    //     mailMessage.To.Add(toEmail);
    //     mailMessage.Subject = subject;
    //     mailMessage.IsBodyHtml = true;
    //     StringBuilder mailBody = new StringBuilder();
    //     mailBody.AppendFormat("<h1>User Registered</h1>");
    //     mailBody.AppendFormat("<br />");
    //     mailBody.AppendFormat("<p>Thank you For Registering account</p>");
    //     mailMessage.Body = mailBody.ToString();
    //
    //     // Send email
    //     await client.SendMailAsync(mailMessage);
    //     // client.Send(mailMessage);
    // }
    
}