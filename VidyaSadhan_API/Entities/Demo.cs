using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Demo
    {
        public Int64 DemoId { get; set; }
        public DateTime DemoDate { get; set; }
        public string TutorId { get; set; }
        public string StudentId { get; set; }

        [StringLength(2)]
        public string DemoType { get; set; }
        public Int64 RatingId { get; set; }
    }
}
