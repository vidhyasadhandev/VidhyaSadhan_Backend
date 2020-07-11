using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class ScheduleType
    {
        [StringLength(4)]
        public string ScheduleTypeId { get; set; }

        [StringLength(255)]
        public string ScheduleTypeName { get; set; }
    }
}
