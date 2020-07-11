using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class AddressViewModel
    {
        public Int64 AddressId { get; set; }

        public string AddressType { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PinCode { get; set; }

        public string StateCd { get; set; }

        public string CountryCd { get; set; }

        public string UserId { get; set; }
    }
}
