using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Classroom.v1.Data;

namespace VS_Models
{
    public class VCourse
    {
        public string Name { get; set; }
        public string Section { get; set; }
        public string DescriptionHeading { get; set; }
        public string Description { get; set; }
        public string Room { get; set; }
        public string OwnerId { get; set; }
        public string CourseState { get; set; }     
    }
}
