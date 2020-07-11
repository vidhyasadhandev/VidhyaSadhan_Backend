using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class InstructorViewModel
    {
        public string UserId { get; set; }
        public UserViewModel Account { get; set; }
        public ICollection<CourseAssignmentViewModel> CourseAssignments { get; set; }
        public ICollection<QuestionnaireViewModel> Questionnaires { get; set; }
    }
}
