using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Address
	{
        [Key]
        public Int64 AddressId { get; set; }

        [StringLength(1)]
        public string AddressType { get; set; }

        [StringLength(255)]
        public string Address1 { get; set; }

        [StringLength(255)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(10)]
        public string PinCode { get; set; }

        [StringLength(4)]
        public string StateCd { get; set; }

        [StringLength(4)]
        public string CountryCd { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Account Account { get; set; }

    }
}
