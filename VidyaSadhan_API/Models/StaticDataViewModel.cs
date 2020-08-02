using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Entities;

namespace VidyaSadhan_API.Models
{
    public class StaticDataViewModel
    {
        public IEnumerable<State> States { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
