using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using VS_Models;

namespace VS_GAPI.Services
{
    public class CourseService : ICourseService
    {
        static string[] Scopes = { ClassroomService.Scope.ClassroomCourses, ClassroomService.Scope.ClassroomRosters };
        static string ApplicationName = "vidhyasadhan-v1";
        
        // ClassroomService _classroomService;

        public CourseService()
        {    
        }
        public ClassroomService Initiate(string user)
        {
            GoogleCredential _credential;

            _credential = GoogleCredential.FromFile("vsadhan-svc.json").CreateScoped(Scopes).CreateWithUser(user);
            var service = new ClassroomService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }


        public List<Course> GetCourses(ClassroomService service)
        {
            CoursesResource.ListRequest request = service.Courses.List();
            request.PageSize = 10;

            ListCoursesResponse response = request.Execute();
            List<Course> vsCourse = new List<Course>();
            if (response.Courses != null && response.Courses.Count > 0)
            {
                foreach (var course in response.Courses)
                {
                    vsCourse.Add(course);
                }
            }
            return vsCourse;
        }

        public Course AddCourse(ClassroomService service, Course course,IEnumerable<Teacher> teachers)
        {
            var crs = service.Courses.Create(course).Execute();
            if (teachers?.Any() == true)
            {
                foreach (var teacher in teachers)
                {
                    AddTeacherToCourse(crs.Id, teacher, service);
                }
            }
            return crs;
        }

        public Teacher AddTeacherToCourse(string courseId, Teacher teacher, ClassroomService classroomService)
        {
            try
            {
               return classroomService.Courses.Teachers.Create(teacher, courseId).Execute();
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("User '{0}' is already a member of this course.\n" +
                            teacher.Profile.EmailAddress);
                }
                else
                {
                    throw e;
                }
            }
            
        }
    }
}
