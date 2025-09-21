

using BookSwap.Core.Helping;
using BookSwap.Application.Abstracts;
using MailKit.Net.Smtp;
using MimeKit;
using BookSwap.Core.Results;
using System.Net.Mail;
using System.Net;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Logging;
using BookSwap.Core.Entities.Identity;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.Application.Implementations
{
    public class EmailService : IEmailService
    {

        #region Failds
        private readonly EmailSettings _emailSettings;
        #endregion
        public ILogger<EmailService> _logger { get; }
        public IHttpContextAccessor _httpContextAccessor { get; }
        public IUrlHelper _urlHelper { get; }
        #region Constructore(s)
        public EmailService(EmailSettings emailSettings ,
                            ILogger<EmailService> logger,
                            IHttpContextAccessor httpContextAccessor,
                            IUrlHelper urlHelper)
        {
            _emailSettings = emailSettings;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

     

        #endregion

        #region Handler Functions
        public async Task<bool> SendEmailAsync(string toEmail, string Message, string? subject)
        {
            try
            {
               // sending the Message of passwordResetLink
                using (var client = new SmtpClient())
                {
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{Message}",
                        TextBody = "wellcome",
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                    message.To.Add(MailboxAddress.Parse(toEmail));
                    message.Subject = subject == null ? "No Submitted" : subject;
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                //end of sending email
                _logger.LogInformation("Email sent to {Email}", toEmail);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                return false;
            }
        }


        public async Task<bool> SendEmailConfirmationAsync(string email, string userId, string confirmationCode)
        {
            var resquestAccessor = _httpContextAccessor.HttpContext!.Request;
            var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host +
                  _urlHelper.Action("ConfirmEmail", "Auth", new { userId = userId, code = confirmationCode });
            var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

            return await SendEmailAsync(email, message, "Confirm Your Email - BookSwap");
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetCode)
        {
            var resquestAccessor = _httpContextAccessor.HttpContext!.Request;
            var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host +
                  _urlHelper.Action("ResetPassword", "Auth", new {code = resetCode });
            var message = $"Code To Reset Passsword : "+ resetCode;

            return await SendEmailAsync(email, message, "Password Reset - BookSwap");

         }
        #endregion
    } 
}

