using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VS_GAPI.Services;
using VS_Models;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly ConfigSettings _configsetting;
        private readonly ILogger<CalendarController> _log;
        public CalendarController(ICalendarService calendarService, 
            IOptionsMonitor<ConfigSettings> optionsMonitor, ILogger<CalendarController> log)
        {
            _calendarService = calendarService;
            _configsetting = optionsMonitor.CurrentValue;
            _log = log;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Events), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult Get(string user)
        {
            try
            {
                var calendar = _calendarService.Initialize(user.IsNullOrWhiteSpace() ? _configsetting.AdminEmail : user);
                return Ok(_calendarService.ListEvents(calendar));
            }
            catch (Exception e)
            {
                _log.LogError(e, e.StackTrace, null);
                throw new VSException(e.StackTrace, e);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(Event), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult Post(CalendarEvent calendarEvent)
        {
            try
            {
                var calendar = _calendarService.Initialize(calendarEvent.UserEmail.IsNullOrWhiteSpace() ? _configsetting.AdminEmail : calendarEvent.UserEmail);
                return Ok(_calendarService.CreateEvent(calendar, calendarEvent));
            }
            catch (Exception e)
            {
                _log.LogError(e, e.StackTrace, null);
                throw new VSException(e.StackTrace, e);
            }

        }
    }
}
