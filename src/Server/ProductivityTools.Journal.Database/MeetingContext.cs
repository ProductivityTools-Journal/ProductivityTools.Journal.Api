using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductivityTools.Meetings.Database.Objects;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace ProductivityTools.Meetings.Database
{
    public class MeetingContext : DbContext
    {
        private readonly IConfiguration configuration;

        public MeetingContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<JournalItem> JournalItem { get; set; }
        public DbSet<JournalItemNotes> JournalItemNotes { get; set; }
        public DbSet<TreeNode> Tree { get; set; }


        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Meetings"));
                optionsBuilder.UseLoggerFactory(GetLoggerFactory());
                optionsBuilder.EnableSensitiveDataLogging();
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("jl");
            modelBuilder.Entity<JournalItem>().HasKey(x => x.JournalItemId);
            modelBuilder.Entity<JournalItemNotes>().HasKey(x => x.JournalItemNotesId);
            modelBuilder.Entity<JournalItemNotes>().Ignore(x=>x.Status);

            modelBuilder.Entity<TreeNode>().ToTable("Tree","jl").HasKey(x => x.TreeId);

           // modelBuilder.Entity<Tree>().HasOne(x => x.Parent).WithMany(x => x.Parent);

            base.OnModelCreating(modelBuilder);
        }


    }
}
