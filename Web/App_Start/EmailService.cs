using CafeT.Text;
using Microsoft.AspNet.Identity;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using CafeT.Html;

namespace Web
{
    public class EmailService : IIdentityMessageService, IDisposable
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = message.Subject;
            _msg.To.Add(new MailAddress(message.Destination));
            _msg.Body = message.Body;
            _msg.IsBodyHtml = true;
            
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(WorkIssue model)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = "[Issue] " + model.Title.ToStandard();
            if (model.CreatedBy.IsEmail())
            {
                _msg.To.Add(new MailAddress(model.CreatedBy));
                _msg.Body = model.Description;
                _msg.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
                })
                {
                    client.Send(_msg);
                }
            }
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}