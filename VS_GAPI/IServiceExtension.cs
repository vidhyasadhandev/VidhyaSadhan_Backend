using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VS_GAPI.Services;

namespace VS_GAPI
{
    public static class IServiceExtension
    {
        public static IServiceCollection AddGAPIConnector(this IServiceCollection services)
        {
           // services.AddTransient<ICourseService, CourseService>();
            return services;
        }
    }
}
