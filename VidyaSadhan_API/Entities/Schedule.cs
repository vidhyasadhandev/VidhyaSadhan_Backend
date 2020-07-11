using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Schedule
    {
        public Int64 ScheduleId { get; set; }
        public string TutorId { get; set; }
        public string StudentId { get; set; }

        [StringLength(2)]
        public string ScheduleType { get; set; }
        public Int64 RatingId { get; set; }
    }
}
