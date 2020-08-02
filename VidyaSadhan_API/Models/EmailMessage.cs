using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class EmailMessage
    {
        public string[] Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public EmailMessage(string[] email, string subject, string message )
        {
            this.Email = email;
            this.Subject = subject;
            this.Message = message;
        }
    }
}
