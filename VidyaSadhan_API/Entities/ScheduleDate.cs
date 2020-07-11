using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class ScheduleDate
    {
        public int ScheduleId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
