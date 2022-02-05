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

        IMeetingService MeetingService => ServiceProvider.GetService<IMeetingService>();
        IMeetingQueries MeetingQueries => ServiceProvider.GetService<IMeetingQueries>();
        IMeetingCommands MeetingCommands => ServiceProvider.GetService<IMeetingCommands>();
        IMapper Mapper => ServiceProvider.GetService<IMapper>();



        [TestMethod]
        public void GetDateTest()
        {
            var controler = new MeetingsController(MeetingQueries, null, null, null, null, null);
            var result = controler?.GetDate();
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetMeetingsTest()
        {

            var controler = new MeetingsController(null, null, MeetingService, null, null, null);
            var result = controler.GetList(new CoreObjects.MeetingListRequest { DrillDown = true, Id = null });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMeetingTest()
        {
            var controler = new MeetingsController(MeetingQueries, null, null, Mapper, null, null);
            var result = controler.Get(new CoreObjects.MeetingId { Id = 1 });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddMeetingTest()
        {
            var controler = new MeetingsController(null, MeetingCommands, null, Mapper, null, null);
            var result = controler.Save(new CoreObjects.JournalItem
            {
                Date = DateTime.Now,
                Subject = "Test Journal",
                TreeId = 1,
                Notes = new System.Collections.Generic.List<CoreObjects.JournalItemNotes>() {
                    new CoreObjects.JournalItemNotes {
                        Type = "xxx",
                        Notes = "Notes" } }
            });
            Assert.IsNotNull(result);
        }
    }
}