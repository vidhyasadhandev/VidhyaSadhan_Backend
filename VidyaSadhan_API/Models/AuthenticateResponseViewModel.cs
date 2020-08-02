using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class AuthenticateResponseViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        //[JsonIgnore] 
        public string RefreshToken { get; set; }

        public AuthenticateResponseViewModel(AccountViewModel user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            Email = user.Email;
        }
    }
}
