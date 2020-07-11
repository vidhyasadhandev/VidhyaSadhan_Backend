using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Student
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Account Account { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
