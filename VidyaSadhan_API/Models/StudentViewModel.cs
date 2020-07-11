using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Models
{
    public class StudentViewModel
    {
        public string UserId { get; set; }
        public UserViewModel Account { get; set; }
        public ICollection<EnrolementViewModel> Enrollments { get; set; }
    }
}
