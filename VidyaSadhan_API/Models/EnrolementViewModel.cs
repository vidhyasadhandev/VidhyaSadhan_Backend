using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Extensions;

namespace VidyaSadhan_API.Models
{
    public class EnrolementViewModel
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }
        public CourseViewModel Course { get; set; }

        public UserViewModel Student { get; set; }
    }
}
