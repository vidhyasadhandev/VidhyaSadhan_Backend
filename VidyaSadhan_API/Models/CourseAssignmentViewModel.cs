using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class CourseAssignmentViewModel
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public InstructorViewModel Instructor { get; set; }
        public CourseViewModel Course { get; set; }
    }
}
