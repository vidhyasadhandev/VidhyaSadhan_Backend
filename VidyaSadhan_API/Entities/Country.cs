using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Country
    {
        [StringLength(4)]
        public string CountryCd { get; set; }

        [StringLength(255)]
        public string CountryName { get; set; }
    }
}
