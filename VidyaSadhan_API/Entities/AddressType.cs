using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class AddressType
    {
        [Key]
        public int TypeId { get; set; }

        [StringLength(255)]
        public string TypeName { get; set; }
    }
}
