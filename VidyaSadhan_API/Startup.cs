using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Extensions;
using VidyaSadhan_API.Helpers;
using VidyaSadhan_API.Helpers.Filters;
using VidyaSadhan_API.Services;
using VS_GAPI;
using VS_GAPI.Services;
using WebPush;
using NSwag.AspNetCore;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("trvistapolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200","http://*.vidhyasadhan.com")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers((options => options.Filters.Add(new HttpResponseExceptionFilter())))
                .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                    return result;
                };
            });

            services.AddDbContext<VSDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
            services.AddIdentity<Account, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.AuthenticatorTokenProvider = "VSadhan";
            }).AddEntityFrameworkStores<VSDbContext>().AddDefaultTokenProviders();

            services.AddTransient<ICourseService, CourseService>();

            services.Configure<ConfigSettings>(Configuration.GetSection("Secrets"));

            var vapidDetails = new VapidDetails(
               Configuration.GetValue<string>("VapidDetails:Subject"),
               Configuration.GetValue<string>("VapidDetails:PublicKey"),
               Configuration.GetValue<string>("VapidDetails:PrivateKey"));
            services.AddTransient(c => vapidDetails);

            //services.AddCertificateForwarding(options =>
            //{
            //    options.CertificateHeader = "X-SSL-CERT";
            //    options.HeaderConverter = (headerValue) =>
            //    {
            //        X509Certificate2 clientCertificate = null;

            //        if (!string.IsNullOrWhiteSpace(headerValue))
            //        {
            //            byte[] bytes = StringToByteArray(headerValue);
            //            clientCertificate = new X509Certificate2(bytes);
            //        }

            //        return clientCertificate;
            //    };
            //});

            var appSettings = Configuration.GetSection("Secrets:AppSecret");
            var securityKey = Encoding.ASCII.GetBytes(appSettings.Value);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
            //            var userId = context.Principal.Identity.Name;
            //            var user = userService.GetUserById(userId.ToString());
            //            if (user == null)
            //            {
            //                context.Fail("Unauthorized");
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));


            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<UserService>();
            services.AddTransient<InstructorService>();
            services.AddTransient<StudentService>();
            services.AddTransient<AddressService>();
            services.AddScoped<ICalendarService, CalenderService>();
            services.AddScoped<StaticService>();
            services.AddSwaggerDocument();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vidhya Sadhan API", Version = "v1" });
            //});

        }

        private byte[] StringToByteArray(string headerValue)
        {
            int NumberChars = headerValue.Length;
            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(headerValue.Substring(i, 2), 16);
            }

            return bytes;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VSDbContext vSDbContext)
        {
          //  vSDbContext.Database.Migrate();
            app.UseCors("trvistapolicy");
            app.UsePreflightRequestHandler();
            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.UseOpenApi();
            app.UseSwaggerUi3();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
