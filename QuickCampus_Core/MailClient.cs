using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core
{
    public class MailClient
    {
        private string? apiKey;
        private string? fromEmail;
        private string toUserName;
        private string subject;
        private string body;

        public MailClient(string apiKey, string fromEmail, string fromUserName, string toEmail, string toUserName, string subject, string body, string token)
        {
            _apiKey = apiKey;
            _fromEmail = fromEmail;
            _fromUserName = fromUserName;
            _toEmail = toEmail;
            _toUserName = toUserName;
            _subject = subject;
            _body = body;
            _token = token;
        }

       
        private static string _apiKey { get; set; }
        private static string _fromEmail { get; set; }
        private static string _fromUserName { get; set; }
        private static string _toEmail { get; set; }
        private static string _toUserName { get; set; }
        private static string _subject { get; set; }
        private static string _body { get; set; }
        private static string _token { get; set; }
        public bool Send() => Task.Run(async () => await Execute()).Result;
        static async Task<bool> Execute()
        {
            try
            {
                SendGridClient client = new SendGridClient(_apiKey);
                EmailAddress from = new EmailAddress(_fromEmail, _fromUserName);
                EmailAddress to = new EmailAddress(_toEmail, _toUserName);
                SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, _subject, _body, _body);
                Response response = await client.SendEmailAsync(msg);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
