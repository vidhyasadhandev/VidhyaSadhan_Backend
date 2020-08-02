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

        public int CourseId { get; set; }
        public string Title { get; set; }

        public int Credits { get; set; }

        public int DepartmentID { get; set; }
        public string AdminId { get; set; }

        public DepartmentViewModel Department { get; set; }
        public ICollection<EnrolementViewModel> Enrollments { get; set; }
        public ICollection<CourseAssignmentViewModel> CourseAssignments { get; set; }

    }
}
