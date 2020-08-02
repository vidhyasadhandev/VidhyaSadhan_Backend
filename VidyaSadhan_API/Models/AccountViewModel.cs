using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class AccountViewModel : UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public List<RefreshTokenViewModel> RefreshTokens { get; set; }

        public List<AddressViewModel> Addresses { get; set; }
    }
}
