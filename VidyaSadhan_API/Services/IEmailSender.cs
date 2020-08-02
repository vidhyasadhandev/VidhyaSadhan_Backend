using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
