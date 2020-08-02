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
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly StudentService _studentService;

        public StudentController(ILogger<StudentController> logger, StudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _studentService.GetAllStudents());
        }

        [HttpGet("userid")]
        [ProducesResponseType(typeof(StudentViewModel), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Get(string userid)
        {
            return Ok(await _studentService.GetStudentById(userid));
        }
    }
}
