using AutoMapper;
using Google.Apis.Classroom.v1.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Models;
using VS_Models;

namespace VidyaSadhan_API.Helpers.Mappers
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel,IdentityUser>().ReverseMap();
            CreateMap<VCourse, Course>().ReverseMap();
            CreateMap<VTeacher, Teacher>().ReverseMap();
            //CreateMap<UpdateModel, User>();
        }
    }
}
