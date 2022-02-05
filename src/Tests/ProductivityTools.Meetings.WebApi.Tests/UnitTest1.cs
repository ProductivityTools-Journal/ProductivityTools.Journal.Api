using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.Meetings.Services;
using ProductivityTools.Meetings.WebApi.Controllers;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;
using System;

namespace ProductivityTools.Meetings.WebApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IConfiguration _config;
        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        public ServiceProvider ServiceProvider
        {
            get
            {
                var services = new ServiceCollection();

                services.AddSingleton<IConfiguration>(Configuration);


                //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                services.AddControllers();
                services.ConfigureServicesTreeService();
                services.ConfigureServicesQueries();
                services.ConfigureServicesCommands();
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                return services.BuildServiceProvider();
            }
        }



        [TestMethod]
        public void GetDateTest()
        {
            IMeetingQueries meetingQueries = ServiceProvider.GetService<IMeetingQueries>();
            var controler = new MeetingsController(meetingQueries, null, null, null, null, null);
            var result = controler?.GetDate();
        }


        [TestMethod]
        public void GetMeetingsTest()
        {
            IMeetingService meetingService = ServiceProvider.GetService<IMeetingService>();
            var controler = new MeetingsController(null, null, meetingService, null, null, null);
            var result = controler.GetList(new CoreObjects.MeetingListRequest { DrillDown = true, Id = null }).Result;

        }
    }
}