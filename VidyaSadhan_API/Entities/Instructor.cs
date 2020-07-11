using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Instructor
    {
        public Instructor()
        {
            //CourseAssignments = new HashSet<CourseAssignment>();
            //Questionnaires = new HashSet<Questionnaire>();
        }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Account Account { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
