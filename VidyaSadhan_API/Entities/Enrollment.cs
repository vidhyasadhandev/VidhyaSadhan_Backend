using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VidyaSadhan_API.Extensions;

namespace VidyaSadhan_API.Entities
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        [ForeignKey("Fk_Std_Crs")]
        public Course Course { get; set; }

        [ForeignKey("Fk_Std_Enr")]
        public Account Student { get; set; }
    }
}