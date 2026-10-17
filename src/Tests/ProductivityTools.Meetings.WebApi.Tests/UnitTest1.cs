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

        IPageService MeetingService => ServiceProvider.GetService<IPageService>();
        IMeetingQueries MeetingQueries => ServiceProvider.GetService<IMeetingQueries>();
        IJournalCommands MeetingCommands => ServiceProvider.GetService<IJournalCommands>();
        ITreeService TreeService => ServiceProvider.GetService<ITreeService>();
        IMapper Mapper => ServiceProvider.GetService<IMapper>();



        //[TestMethod]
        //public void GetDateTest()
        //{
        //    var controler = new PageController(MeetingQueries, null, null,null, null, null, null);
        //    var result = controler?.GetDate();
        //    Assert.IsNotNull(result);
        //}


        //[TestMethod]
        //public void GetMeetingsTest()
        //{

        //    var controler = new PageController(null, null, MeetingService, null,null, null, null);
        //    var result = controler.GetList(new CoreObjects.MeetingListRequest { DrillDown = true, Id = null });
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void GetMeetingTest()
        //{
        //    var controler = new MeetingsController(MeetingQueries, null, null, Mapper, null, null);
        //    var result = controler.Get(new CoreObjects.JournalId { Id = 1 });
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void AddMeetingTest()
        //{
        //    var controler = new MeetingsController(null, MeetingCommands, null, Mapper, null, null);
        //    var result = controler.Save(new CoreObjects.Page
        //    {
        //        Date = DateTime.Now,
        //        Subject = "Test Journal",
        //        JournalId = 1,
        //        NotesList = new System.Collections.Generic.List<CoreObjects.JournalItemNotes>() {
        //            new CoreObjects.JournalItemNotes {
        //                Type = "xxx",
        //                Notes = "Notes" } }
        //    });
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void DeleteJournalItemDetails()
        //{
        //    var controler = new MeetingsController(MeetingQueries, MeetingCommands,MeetingService, Mapper, null, null);
        //    var journalList = controler.GetList(new CoreObjects.MeetingListRequest { DrillDown = true, Id = null });
        //    var journalElement = journalList[0];
        //    var lastElement = journalElement.NotesList.Last();
        //    lastElement.Status = "Deleted";

        //    controler.Update(journalElement);
            
        //}

        //[TestMethod]
        //public void MoveTreeItem()
        //{
        //    var treeController = new TreeController(TreeService,Mapper);


        //    var tree=treeController.GetTree();
        //    var parent = tree.First().Nodes.Last();

        //    var treeId=treeController.AddTreeNode(new NewTreeNodeRequest() {  Name = "Test", ParentId = parent.Id });

        //    MoveTreeRequest request = new MoveTreeRequest();
        //    request.SourceId = treeId;
        //    request.ParentTargetId = tree.First().Id;

        //    var r= treeController.MoveTree(request);
        //    Assert.IsInstanceOfType(r, typeof(OkResult));

        //}

        [TestMethod]
        public void GetTree_FiltersArchivedByDefault_AndIncludesWhenRequested()
        {
            var fakeQueries = new FakeTreeQueries();
            var fakeCommands = new FakeTreeCommands(fakeQueries.Journals);
            var treeService = new TreeService(fakeQueries, fakeCommands, null!, null!, null!, Mapper);

            var defaultTree = treeService.GetTree("test@example.com");
            Assert.IsNotNull(defaultTree);
            Assert.AreEqual(1, defaultTree.Nodes.Count);
            Assert.AreEqual("Active Node", defaultTree.Nodes[0].Name);
            Assert.IsFalse(defaultTree.Nodes[0].Archived);

            var allTree = treeService.GetTree("test@example.com", includeArchived: true);
            Assert.IsNotNull(allTree);
            Assert.AreEqual(2, allTree.Nodes.Count);
            var archivedNode = allTree.Nodes.Single(x => x.Name == "Archived Node");
            Assert.IsTrue(archivedNode.Archived);
            Assert.AreEqual(1, archivedNode.Nodes.Count);
            Assert.AreEqual("Child Of Archived", archivedNode.Nodes[0].Name);
        }

        [TestMethod]
        public void ArchiveJournal_TogglesArchivedFlag()
        {
            var fakeQueries = new FakeTreeQueries();
            var fakeCommands = new FakeTreeCommands(fakeQueries.Journals);
            var treeService = new TreeService(fakeQueries, fakeCommands, null!, null!, null!, Mapper);

            var archived = treeService.ArchiveJournal("test@example.com", 2, true);
            Assert.IsTrue(archived.Archived);
            Assert.AreEqual(0, treeService.GetTree("test@example.com", includeArchived: false).Nodes.Count);
            Assert.AreEqual(2, treeService.GetTree("test@example.com", includeArchived: true).Nodes.Count);

            var unarchived = treeService.ArchiveJournal("test@example.com", 2, false);
            Assert.IsFalse(unarchived.Archived);
            Assert.AreEqual(1, treeService.GetTree("test@example.com", includeArchived: false).Nodes.Count);
        }

        private class FakeTreeQueries : ITreeQueries
        {
            public System.Collections.Generic.List<Database.Objects.Journal> Journals { get; } = new()
            {
                new Database.Objects.Journal { JournalId = 1, ParentId = 1, Name = "Root", Deleted = false, Archived = false },
                new Database.Objects.Journal { JournalId = 2, ParentId = 1, Name = "Active Node", Deleted = false, Archived = false },
                new Database.Objects.Journal { JournalId = 3, ParentId = 1, Name = "Archived Node", Deleted = false, Archived = true },
                new Database.Objects.Journal { JournalId = 4, ParentId = 3, Name = "Child Of Archived", Deleted = false, Archived = false }
            };

            public Database.Objects.Journal GetRoot() => Journals.First(x => x.JournalId == 1);
            public System.Collections.Generic.List<Database.Objects.Journal> GetTree(string email, int parentId, bool includeArchived = false)
                => Journals.Where(x => x.ParentId == parentId && x.JournalId != x.ParentId && !x.Deleted && (includeArchived || !x.Archived)).ToList();
            public System.Collections.Generic.List<Database.Objects.Journal> GetUserJournals(string email, bool includeArchived = false)
                => Journals.Where(x => x.JournalId != 1 && !x.Deleted && (includeArchived || !x.Archived)).ToList();
            public Database.Objects.Journal GetTreeNode(int id) => Journals.Single(x => x.JournalId == id);
            public bool ValidateOnershipCall(string email, int[] treeIds) => true;
            public Database.Objects.Journal GetJournalByPublicHash(string publicHash) => null!;
            public System.Collections.Generic.List<Database.Objects.Journal> GetChildJournals(int parentId, bool includeArchived = false) => new();
            public string GetJournalPath(int journalId) => string.Empty;
            public System.Collections.Generic.Dictionary<int, string> GetJournalPaths(System.Collections.Generic.IEnumerable<int> journalIds) => new();
            public System.Collections.Generic.List<int> GetFlatChildsIdPublic(int parentId, bool includeArchived = false) => new();
        }

        private class FakeTreeCommands : ITreeCommands
        {
            private readonly System.Collections.Generic.List<Database.Objects.Journal> _journals;
            public FakeTreeCommands(System.Collections.Generic.List<Database.Objects.Journal> journals) => _journals = journals;
            public Database.Objects.Journal AddTreeNode(int parentId, string name) => throw new NotImplementedException();
            public int Delete(System.Collections.Generic.IEnumerable<int> treeIds) => throw new NotImplementedException();
            public void Move(int sourceId, int targetId) => throw new NotImplementedException();
            public Database.Objects.Journal RenameJournal(int journalId, string newName) => throw new NotImplementedException();
            public int? CheckIfTreeNodeExists(int parentId, string name) => throw new NotImplementedException();
            public string GetPublicHash(int journalId) => throw new NotImplementedException();
            public Database.Objects.Journal SetInboxName(int journalId, string inboxName) => throw new NotImplementedException();
            public Database.Objects.Journal RemoveInboxName(int journalId) => throw new NotImplementedException();
            public Database.Objects.Journal ArchiveJournal(int journalId, bool archived = true)
            {
                var j = _journals.Single(x => x.JournalId == journalId);
                j.Archived = archived;
                return j;
            }
        }
    }
}