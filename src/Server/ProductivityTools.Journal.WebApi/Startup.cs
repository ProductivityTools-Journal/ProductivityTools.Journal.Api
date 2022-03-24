using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductivityTools.Meetings.Services;
using ProductivityTools.Meetings.WebApi.Controllers;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;

namespace ProductivityTools.Meetings.WebApi
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();
            services.ConfigureServicesTreeService();
            services.ConfigureServicesQueries();
            services.ConfigureServicesCommands();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

           // string domain = $"https://demo.identityserver.io/api/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.Authority = "https://identityserver.productivitytools.tech:8010";
                options.Authority = "https://securetoken.google.com/ptlearning-95d51";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/ptlearning-95d51",
                    ValidateAudience = true,
                    ValidAudience = "ptlearning-95d51",
                    ValidateLifetime = true
                };
            });
            services.AddDefaultIdentity<IdentityUser>();

            services.AddMvc(opt=>opt.EnableEndpointRouting=false);//pw:maybe delete after auth will work

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://meetingsweb.z13.web.core.windows.net").AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
          
            app.UseRouting();
            app.UseAuthentication();
            app.UseMvc();//maybe to delete aftera auth will work

            // app.UseCors(builder => builder.WithOrigins("http://localhost:3000/").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
           
            

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
