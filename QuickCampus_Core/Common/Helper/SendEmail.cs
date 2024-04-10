using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using QuickCampus_Core.ViewModel;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Common.Helper
{
    public class SendEmail
    {
        private readonly IConfiguration _config;
        private readonly ViewModel.MailSettings _mailSettings;

        public SendEmail(IConfiguration config, IOptions<ViewModel.MailSettings> mailSettings)
        {
            _config = config;
            _mailSettings = mailSettings.Value;
        }
        public IGeneralResult<string> SendGridEmail(SendMailViewModel model)
        {
            IGeneralResult<string> result = new GeneralResult<string>();
            
            var fromEmail = _config["MailSettings:fromEmail"];
            var fromUserName = _config["MailSettings:fromUserName"];
            _mailSettings.SendGridApiKey = _config["MailSettings:sendGridApiKey"];
            MailClient mailClient = new MailClient(apiKey: _mailSettings.SendGridApiKey, fromEmail: fromEmail,
               fromUserName: fromUserName, toEmail: model.ReceiverEmailId,token:"",
                toUserName: model.ReceiverEmailId, subject: model.Subject, body: model.Body);
            bool sent = mailClient.Send();
            if (sent == true)
            {
                result.IsSuccess = true;
                result.Message = "Mail sent successfully on " + model.ReceiverEmailId;
            }
            else
            {
                result.Message = "Something went wrong! Please try again.";
            }
            return result;
        }
         
    }
    
}
    

