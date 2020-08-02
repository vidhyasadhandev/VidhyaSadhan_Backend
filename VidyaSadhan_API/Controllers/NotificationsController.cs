using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VidyaSadhan_API.Models;
using VidyaSadhan_API.Services;
using WebPush;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        public static List<PushSubscription> Subscriptions { get; set; } = new List<PushSubscription>();
        private readonly IEmailSender _emailSender;
        public NotificationsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("subscribe")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void Subscribe([FromBody] PushSubscription sub)
        {
            Subscriptions.Add(sub);
        }

        [HttpPost("unsubscribe")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void Unsubscribe([FromBody] PushSubscription sub)
        {
            var item = Subscriptions.FirstOrDefault(s => s.Endpoint == sub.Endpoint);
            if (item != null)
            {
                Subscriptions.Remove(item);
            }
        }

        [HttpPost("broadcast")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public void Broadcast([FromBody] NotificationModel message, [FromServices] VapidDetails vapidDetails)
        {
            var client = new WebPushClient();
            var serializedMessage = JsonConvert.SerializeObject(message);
            foreach (var pushSubscription in Subscriptions)
            {
                client.SendNotification(pushSubscription, serializedMessage, vapidDetails);
            }

        }

        [HttpPost("email")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult SendEmail([FromBody] EmailMessage message)
        {
            try
            {
                _emailSender.SendEmailAsync(message);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
           
        }
    }
}
