using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;
using VidyaSadhan_API.Services;

namespace VidyaSadhan_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]  
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, UserService userService, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(typeof(AuthenticateResponseViewModel), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginViewModel model)
        {
            try
            {
                model.IpAddress = GetIPAddress();
                var response = await _userService.Login(model).ConfigureAwait(false);
                SetTokenCookie(response.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new VSException(ex.StackTrace,ex);
            }
            
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> LogOut()
        {
            return Ok(await _userService.LogOut().ConfigureAwait(false));
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
        [Route("confirm")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> Confirm(string userId, string token)
        {
            return Ok(await _userService.ConfirmEmail(userId,token).ConfigureAwait(false));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reconfirm")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> ReConfirm(string emailid)
        {
            await _userService.GenerateEmailToken(emailid).ConfigureAwait(false);
            return Ok(true);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(IEnumerable<AccountViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult Get(string id)
        {
            try
            {
                return Ok(_userService.GetUserById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured in users", null);
                throw;
            }

        }

        [HttpGet]
        [Route("allusers")]
        [ProducesResponseType(typeof(IEnumerable<AccountViewModel>), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error Occured in users",null);
                throw;
            }
            
        }

        [HttpGet]
        [Route("byemail/{email}")]
        [ProducesResponseType(typeof(AccountViewModel), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public IActionResult GetUserByEmailId(string email)
        {
            return Ok(_userService.GetUserByEmailId(email));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> DeleteUser([FromBody] AccountViewModel model)
        {
            return Ok(await (_userService.DeleteUser(model).ConfigureAwait(false)));
        }

        [AllowAnonymous]
        [Route("refreshtoken")]
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPost]
        public async Task<IActionResult> Refresh(RevokTokenViewModel token)
        {
           // var refreshToken = Request.Cookies["refreshToken"];
            var response = await _userService.RefreshToken(token.Token, GetIPAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpGet("{id}/refreshtokens")]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            return Ok(user.RefreshTokens);
        }

        // helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        [AllowAnonymous]
        [Route("revoketoken")]
        [ProducesResponseType(typeof(bool), 200)]
        [HttpPost]
        public async Task<IActionResult> Revoke([FromBody] RevokTokenViewModel rvtoken)
        {
            var token = rvtoken.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _userService.RevokeToken(token, GetIPAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        [Route("roles/{role}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesErrorResponseType(typeof(VSException))]
        public async Task<IActionResult> CreateRoles(UserRoles role)
        {
            return Ok(await (_userService.AddUserRoles(role).ConfigureAwait(false)));
        }

        private string GetIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
