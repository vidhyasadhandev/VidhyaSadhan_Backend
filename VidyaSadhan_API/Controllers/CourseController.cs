using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.Classroom.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VS_GAPI.Services;
using VS_Models;

namespace VidyaSadhan_API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseService _courseService;
        private readonly ConfigSettings _configsetting;
        IMapper _mapper;
        public CourseController(ILogger<CourseController> logger, ICourseService courseService, 
            IMapper mapper, IOptionsMonitor<ConfigSettings> optionsMonitor)
        {
            _courseService = courseService;
            _logger = logger;
            _mapper = mapper;
            _configsetting = optionsMonitor.CurrentValue;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("allcourses")]
        [ProducesResponseType(typeof(IEnumerable<VCourse>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetAllCourses()
        {
            var classroom = _courseService.Initiate(_configsetting.AdminEmail);
            return Ok(_mapper.Map<IEnumerable<VCourse>>(_courseService.GetCourses(classroom)));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(VCourse), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult CreateCourse(CourseViewModel courseViewModel)
        {
            var classroom = _courseService.Initiate(courseViewModel.AdminId.IsNullOrWhiteSpace() ? _configsetting.AdminEmail : courseViewModel.AdminId);
            return Ok(_mapper.Map<VCourse>(_courseService.AddCourse(classroom, _mapper.Map<Course>(courseViewModel.VCourse),
                _mapper.Map<IEnumerable<Teacher>>(courseViewModel.VTeachers))));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("addteacher/{courseId}")]
        [ProducesResponseType(typeof(VTeacher), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult AddTeacherToCourse(string courseId, [FromBody] VTeacher teacher)
        {
            var classroom = _courseService.Initiate(_configsetting.AdminEmail);
            return Ok(_mapper.Map<VTeacher>(_courseService.AddTeacherToCourse(courseId, _mapper.Map<Teacher>(teacher), classroom)));
        }
    }
}
