using EmailMarketing.Framework.Entities.SMTP;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Services
{
    public class SmtpTestService : ISmtpTestService
    {

        private readonly IWebHostEnvironment _env;

        public SmtpTestService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<bool> TestSmtpSettings(SMTPConfig sMTPConfig)
        {
            try
            {

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //if (_env.IsDevelopment())
                    //{
                    await client.ConnectAsync(sMTPConfig.Server, sMTPConfig.Port, true);
                    //}
                    //else
                    //{
                    //    await client.ConnectAsync(sMTPConfig.Server);
                    //}

                    await client.AuthenticateAsync(sMTPConfig.UserName, sMTPConfig.Password);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
