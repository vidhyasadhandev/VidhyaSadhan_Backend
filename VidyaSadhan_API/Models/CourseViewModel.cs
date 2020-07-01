using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS_Models;

namespace VidyaSadhan_API.Models
{
    public class CourseViewModel
    {
        public VCourse VCourse { get; set; }
        public IEnumerable<VTeacher> VTeachers { get; set; }
    }
}
