using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using System;
using System.Collections.Generic;
using System.Text;
using VS_Models;

namespace VS_GAPI.Services
{
    public interface ICourseService
    {
        ClassroomService Initiate(string user);
        List<Course> GetCourses(ClassroomService service);
        Course AddCourse(ClassroomService service, Course course, IEnumerable<Teacher> teachers);
        Teacher AddTeacherToCourse(string courseId, Teacher teacher, ClassroomService classroomService);
    }
}
