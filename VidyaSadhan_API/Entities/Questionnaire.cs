using System.Collections.Generic;

namespace VidyaSadhan_API.Entities
{
    public class Questionnaire
    {
        public int Id { get; set; }
        public int InstructorID { get; set; }

        public Instructor Instructor { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}