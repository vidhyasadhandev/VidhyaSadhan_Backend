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
    [Authorize]
    [ApiController]
    [Route("[controller]")]  
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginViewModel model)
        {
            return Ok(await _userService.Login(model).ConfigureAwait(false));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> LogOut([FromBody] UserViewModel model)
        {
            return Ok(await _userService.LogOut(model).ConfigureAwait(false));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(IEnumerable<bool>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            return Ok(await _userService.Register(model).ConfigureAwait(false));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("allusers")]
        [ProducesResponseType(typeof(IEnumerable<UserViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet]
        [Route("byemail/{email}")]
        [ProducesResponseType(typeof(UserViewModel), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetUserByEmailId(string email)
        {
            return Ok(_userService.GetUserByEmailId(email));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> DeleteUser([FromBody] UserViewModel model)
        {
            return Ok(await (_userService.DeleteUser(model).ConfigureAwait(false)));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("roles/{role}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> CreateRoles(UserRoles role)
        {
            return Ok(await (_userService.AddUserRoles(role).ConfigureAwait(false)));
        }
    }
}
