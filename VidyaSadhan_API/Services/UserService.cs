using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VidyaSadhan_API.Extensions;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public class UserService
    {
        private VSDbContext _identityContext;
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;
        private readonly ConfigSettings _configsetting;
        private RoleManager<IdentityRole> _roleManager;
        IMapper _map;

        public UserService(VSDbContext identityContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IMapper map,
            IOptionsMonitor<ConfigSettings> optionsMonitor,
            RoleManager<IdentityRole> roleManager)
        {
            _identityContext = identityContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configsetting = optionsMonitor.CurrentValue;
            _map = map;
            _roleManager = roleManager;
        }

        public async Task<bool> Register(RegisterViewModel register)
        {
            var IsUserExist = _identityContext.Users.Any(user => user.Email.Equals(register.Email, StringComparison.OrdinalIgnoreCase));

            if (IsUserExist)
            {
                throw new VSException("Email already registered with:" + register.Email);
            }

            var User = new IdentityUser
            {
                UserName = register.Email,
                Email = register.Email,
                PhoneNumber = register.Phone,
                EmailConfirmed = true,
            };

            IdentityResult results = null;
            try
            {
                results = await _userManager.CreateAsync(User, register.Password).ConfigureAwait(false);
                if (results.Succeeded)
                {
                    await _userManager.AddToRoleAsync(User, register.Role.ToString());
                }
                return results.Succeeded;
            }
            catch (Exception)
            { 
                throw new VSException("Unable to Register With Following Errors:", results?.Errors);
            }
        }

        public async Task<UserViewModel> Login(LoginViewModel login)
        {
            var userexists = _identityContext.Users.SingleOrDefault(u => u.Email.Equals(login.Email, StringComparison.OrdinalIgnoreCase));
            if (userexists == null)
            {
                var exception = new VSException("Looks like Email is not registered");
                exception.Value = "Looks like Email is not registered";
                throw exception;
            }

            var results = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false).ConfigureAwait(false);

            if (results.Succeeded)
            {
                var user = _map.Map<UserViewModel>(userexists);
                user.Token = authenticateToken(userexists);
                return user;
            }

            if (results.IsLockedOut)
            {
                var exception = new VSException("Your Account is Locked");
                exception.Value = "Your Account is Locked";
                throw exception;
            }
            else if (results.IsNotAllowed)
            {
                var exception = new VSException("Invalid Attempt to Login");
                exception.Value = "Invalid Attempt to Login";
                throw exception;
            }
            else
            {
                var exception = new VSException("Invalid Attempt to Login");
                exception.Value = "Invalid Attempt to Login";
                throw exception;
            }
        }

        public async Task<bool> LogOut(UserViewModel user)
        {
            try
            {
                await _signInManager.SignOutAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                var exception = new VSException("error occured");
                exception.Value = "Unable To Logout";
                throw exception;
            }
        }
        public IEnumerable<UserViewModel> GetAllUsers()
        {
            try
            {
                var results = _map.Map<IEnumerable<UserViewModel>>(_identityContext.Users);
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }
        }

        public UserViewModel GetUserById(string user)
        {
            try
            {
                var results = _map.Map<UserViewModel>(_identityContext.Users.SingleOrDefault(x => x.Id == user));
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public UserViewModel GetUserByEmailId(string user)
        {
            try
            {
                var results = _map.Map<UserViewModel>(_identityContext.Users.SingleOrDefault(x => x.Email == user));
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public UserViewModel GetUserByRole(UserViewModel user)
        {
            try
            {
                var results = _map.Map<UserViewModel>(_identityContext.Users.SingleOrDefault(x => x.UserName == user.UserName));
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public async Task<bool> DeleteUser(UserViewModel user)
        {
            var IsUserExist = _identityContext.Users.FirstOrDefault(user => user.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

            if (IsUserExist == null)
            {
                throw new VSException("User Not Exists to Delete");
            }

            IdentityResult results = null;
            try
            {
                results = await _userManager.DeleteAsync(IsUserExist).ConfigureAwait(false);
                if(results.Succeeded == false)
                {
                    if (results?.Errors.Any() == true)
                    {
                        var exception = new VSException("error occured");
                        exception.Value = string.Join(';', results.Errors);
                        throw exception;
                    }
                }
                return results.Succeeded;
            }
            catch (Exception)
            {
                throw new VSException("Unable to Delete the User with following errors", results?.Errors);
            }
        }

        public string authenticateToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configsetting.AppSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<bool> AddUserRoles(UserRoles Role)
        {
            var existingRoles = await _roleManager.FindByNameAsync(Role.ToString()).ConfigureAwait(false);
            if(existingRoles != null)
            {
                var exception = new VSException("Role Already Existing");
                exception.Value = "Role Already Existing:" + Role.ToString();
                throw exception;
            }

            var results = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = Role.ToString(),
            }).ConfigureAwait(false);

            if (results?.Errors.Any() == true)
            {
                var exception = new VSException("error occured");
                exception.Value = string.Join(';', results.Errors);
                throw exception;
            }

            return results.Succeeded;
        }
    }
}
