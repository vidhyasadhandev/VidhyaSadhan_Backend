using System;
using gap = Google.Cloud.Iam.V1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using VS_Models;

namespace VS_GAPI
{
    public class ClassRoomAPI
    {
        static string[] Scopes = { ClassroomService.Scope.ClassroomCoursesReadonly };
        static string ApplicationName = "Vidhya Sadhan Class Room";
        public ClassRoomAPI()
        {
           
        }

        public List<VCourse> Initiate()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("VidhyaSadhan-4cc497c45b73.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            var service = new ClassroomService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            CoursesResource.ListRequest request = service.Courses.List();
            request.PageSize = 10;

            // List courses.
            ListCoursesResponse response = request.Execute();
            List<VCourse> vsCourse = new List<VCourse>();
            if (response.Courses != null && response.Courses.Count > 0)
            {
                foreach (var course in response.Courses)
                {
                    vsCourse.Add((VCourse)course);
                }
            }
            
            return vsCourse;
        }
    }
}
