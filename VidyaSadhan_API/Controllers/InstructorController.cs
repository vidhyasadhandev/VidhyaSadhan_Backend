using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VidyaSadhan_API.Services;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ILogger<InstructorController> _logger;
        private readonly InstructorService _instructorService; 

        public InstructorController(ILogger<InstructorController> logger, InstructorService instructorService)
        {
            _logger = logger;
            _instructorService = instructorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstructorViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _instructorService.GetAllInstructors());
        }

        [HttpGet("userid")]
        [ProducesResponseType(typeof(InstructorViewModel), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Get(string userid)
        {
            return Ok(await _instructorService.GetInstructorById(userid));
        }
    }
}
