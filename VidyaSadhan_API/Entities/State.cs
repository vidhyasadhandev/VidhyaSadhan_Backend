using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class State
    {
        [StringLength(4)]
        public string StateCd { get; set; }

        [StringLength(255)]
        public string StateName { get; set; }
    }
}
