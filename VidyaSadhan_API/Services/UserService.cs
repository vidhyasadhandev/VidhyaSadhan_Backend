using AutoMapper;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Extensions;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public class UserService
    {
        private VSDbContext _identityContext;
        UserManager<Account> _userManager;
        SignInManager<Account> _signInManager;
        private readonly ConfigSettings _configsetting;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        IMapper _map;
        private readonly InstructorService _instructorService;
        private readonly IEmailSender _emailSender;

        public UserService(VSDbContext identityContext,
            UserManager<Account> userManager,
            SignInManager<Account> signInManager, IMapper map,
            IOptionsMonitor<ConfigSettings> optionsMonitor,
            RoleManager<IdentityRole> roleManager,
            InstructorService instructorService,
            ILogger<UserService> logger,
            IEmailSender emailSender)
        {
            _identityContext = identityContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _configsetting = optionsMonitor.CurrentValue;
            _map = map;
            _roleManager = roleManager;
            _instructorService = instructorService;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<bool> Register(RegisterViewModel register)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(register.Email).ConfigureAwait(false);

            if (IsUserExist != null)
            {
                throw new VSException("Email already registered with:" + register.Email);
            }

            var User = new Account
            {
                UserName = register.UserName,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                PhoneNumber = register.Phone
            };

            IdentityResult results = null;
            try
            {
                results = await _userManager.CreateAsync(User, register.Password).ConfigureAwait(false);
                if (results.Succeeded)
                {            
                    await _userManager.AddToRoleAsync(User, register.Role.ToString());
                    switch (register.Role)
                    {
                        case UserRoles.Student:
                            _identityContext.Students.Add(new Student { UserId = User.Id });
                            break;
                        case UserRoles.Tutor:
                            _identityContext.Instructors.Add(new Instructor { UserId = User.Id });
                            break;
                        case UserRoles.Parent:
                        case UserRoles.Admin:
                        default:
                            return results.Succeeded;
                    }               
                    await _identityContext.SaveChangesAsync().ConfigureAwait(false);
                    await GenerateEmailToken(User.Email);
                    // _identityContext.AccountAddress.Add(new Address {   })
                }
                return results.Succeeded;
            }
            catch (Exception)
            {
                throw new VSException("Unable to Register With Following Errors:", results?.Errors);
            }
        }

        public async Task GenerateEmailToken(string emailId)
        {
            var user = await _userManager.FindByEmailAsync(emailId);
            user.VerificationToken = RandomTokenGen();
            _identityContext.Users.Update(user);
            await _identityContext.SaveChangesAsync();
            string url = _configsetting.WebUrl + "/verifyemail";
            var param = new Dictionary<string, string>() { { "userid", user.Id },{ "token", user.VerificationToken } };

            var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));

            var message = new EmailMessage(new string[] { user.Email }, "Confirmation email link", url);
            await _emailSender.SendEmailAsync(message);
        }

        private string RandomTokenGen()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<bool> ConfirmEmail(string userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user != null)
            {
                if (user.VerificationToken == token)
                {
                    user.EmailConfirmed = true;
                    _identityContext.Users.Update(user);
                    await _identityContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogError("Error Occured in Confirming Email");
                    return false;
                }
            }
            else
            {
                _logger.LogError("Looks Like you are not registered");
                return false;
            }   
        }

        public async Task<AuthenticateResponseViewModel> Login(LoginViewModel login)
        {
            try
            {
                var userexists = await _userManager.FindByEmailAsync(login.Email);
                if (userexists == null)
                {
                    var exception = new VSException("Looks like Email is not registered");
                    exception.Value = "Looks like Email is not registered";
                    throw exception;
                }

                var results = await _signInManager.PasswordSignInAsync(userexists.UserName, login.Password, login.RememberMe, false).ConfigureAwait(false);

                if (results.Succeeded)
                {
                    var user = _map.Map<AccountViewModel>(userexists);
                    // user.Token = authenticateToken(userexists);
                    var jwtToken = GenerateJwtToken(user);
                    var refreshToken = GenerateRefreshToken(login.IpAddress);
                    userexists.RefreshTokens.Add(refreshToken);
                    _identityContext.Update(userexists);
                    _identityContext.SaveChanges();
                    return new AuthenticateResponseViewModel(user, jwtToken, refreshToken.Token);
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
            catch (Exception ex)
            {
                var exception = new VSException("System Error Occured", ex);
                exception.Value = ex.Message;
                throw exception;
            }

        }

        private RefreshTokenSet GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenSet
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        public async Task<AuthenticateResponseViewModel> RefreshToken(string token, string ipAddress)
        {
            var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive) return null;

            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync().ConfigureAwait(false);

            AccountViewModel accountView = _map.Map<AccountViewModel>(user);
            var jwtToken = GenerateJwtToken(accountView);

            return new AuthenticateResponseViewModel(accountView, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            var user = _identityContext.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(AccountViewModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configsetting.AppSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> LogOut()
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
        public IEnumerable<AccountViewModel> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("User Info", _identityContext);
                var results = _map.Map<IEnumerable<AccountViewModel>>(_identityContext.Users);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserError", ex);
                throw new VSException("Unable to load Users", ex);
            }
        }

        public AccountViewModel GetUserById(string user)
        {
            try
            {
                var userdata = _identityContext.Users.Include("Addresses").SingleOrDefault(x => x.Id == user);
                var results = _map.Map<AccountViewModel>(userdata);
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public AccountViewModel GetUserByEmailId(string user)
        {
            try
            {
                var results = _map.Map<AccountViewModel>(_identityContext.Users.SingleOrDefault(x => x.Email == user));
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public AccountViewModel GetUserByRole(UserViewModel user)
        {
            try
            {
                var results = _map.Map<AccountViewModel>(_identityContext.Users.SingleOrDefault(x => x.UserName == user.UserName));
                return results;
            }
            catch (Exception ex)
            {
                throw new VSException("Unable to load Users", ex);
            }

        }

        public async Task<bool> DeleteUser(AccountViewModel user)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(user.Email).ConfigureAwait(false);

            if (IsUserExist == null)
            {
                throw new VSException("User Not Exists to Delete");
            }

            IdentityResult results = null;
            try
            {
                results = await _userManager.DeleteAsync(IsUserExist).ConfigureAwait(false);
                if (results.Succeeded == false)
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

        public string authenticateToken(Account user)
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
            if (existingRoles != null)
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

        public async Task<UserViewModel> RefreshToken(string token)
        {
            ClaimsPrincipal claims = GetPrincipalFromExpiredToken(token);
            var username = claims.Identity.Name;
            var user = await _userManager.FindByIdAsync(username).ConfigureAwait(false);
            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", newRefreshToken);
            var userModel = _map.Map<AccountViewModel>(user);
           // userModel.RefreshTokens.Add(newRefreshToken);
            return userModel;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configsetting.AppSecret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }

}
