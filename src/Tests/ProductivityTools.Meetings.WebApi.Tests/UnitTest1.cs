using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.Services;
using ProductivityTools.Meetings.WebApi.Controllers;
using ProducvitityTools.Meetings.Commands;
using ProducvitityTools.Meetings.Queries;
using System;
using System.Linq;

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
        ITreeService TreeService => ServiceProvider.GetService<ITreeService>();
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
                NotesList = new System.Collections.Generic.List<CoreObjects.JournalItemNotes>() {
                    new CoreObjects.JournalItemNotes {
                        Type = "xxx",
                        Notes = "Notes" } }
            });
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteJournalItemDetails()
        {
            var controler = new MeetingsController(MeetingQueries, MeetingCommands,MeetingService, Mapper, null, null);
            var journalList = controler.GetList(new CoreObjects.MeetingListRequest { DrillDown = true, Id = null });
            var journalElement = journalList[0];
            var lastElement = journalElement.NotesList.Last();
            lastElement.Status = "Deleted";

            controler.Update(journalElement);
            
        }

        [TestMethod]
        public void MoveTreeItem()
        {
            var treeController = new TreeController(TreeService,Mapper);


            var tree=treeController.GetTree();
            var parent = tree.First().Nodes.Last();

            var treeId=treeController.AddTreeNode(new NewTreeNodeRequest() {  Name = "Test", ParentId = parent.Id });

            MoveTreeRequest request = new MoveTreeRequest();
            request.SourceId = treeId;
            request.ParentTargetId = tree.First().Id;

            var r= treeController.MoveTree(request);
            Assert.IsInstanceOfType(r, typeof(OkResult));

        }
    }
}