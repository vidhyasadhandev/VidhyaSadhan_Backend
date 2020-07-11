using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public int InstructorID { get; set; }

        public InstructorViewModel Instructor { get; set; }

        public ICollection<QuestionViewModel> Questions { get; set; }
    }
}
