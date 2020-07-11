using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Rating
    {
        public Int64 RatingId { get; set; }
        public int Stars { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
