using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VidyaSadhan_API.Services;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/resourses")]
    [ApiController]
    public class StaticDataController : ControllerBase
    {
        private readonly ILogger<StaticDataController> _log;
        private readonly StaticService _staticData;
        public StaticDataController(ILogger<StaticDataController> log, StaticService staticData)
        {
            _log = log;
            _staticData = staticData;
        }

        [HttpGet]
        [Route("countries")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult Get(string user)
        {
            try
            {
                return Ok(_staticData.GetCountries());
            }
            catch (Exception e)
            {
                _log.LogError(e, e.StackTrace, null);
                throw new VSException(e.StackTrace, e);
            }

        }

        [HttpGet]
        [Route("states")]
        [ProducesResponseType(typeof(IEnumerable<State>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetStates()
        {
            try
            {
                return Ok(_staticData.GetStates());
            }
            catch (Exception e)
            {
                _log.LogError(e, e.StackTrace, null);
                throw new VSException(e.StackTrace, e);
            }

        }

        [HttpGet]
        [Route("staticdata")]
        [ProducesResponseType(typeof(StaticDataViewModel), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetStaticData()
        {
            try
            {
                var staticData = new StaticDataViewModel
                {
                    States = _staticData.GetStates(),
                    Countries = _staticData.GetCountries()
                };
                return Ok(staticData);
            }
            catch (Exception e)
            {
                _log.LogError(e, e.StackTrace, null);
                throw new VSException(e.StackTrace, e);
            }

        }

    }
}
