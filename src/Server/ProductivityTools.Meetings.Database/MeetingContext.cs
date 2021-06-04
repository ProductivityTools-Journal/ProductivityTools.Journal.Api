using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductivityTools.Meetings.Database.Objects;
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

        public DbSet<Meeting> Meeting { get; set; }
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
            modelBuilder.HasDefaultSchema("mt");
            modelBuilder.Entity<Meeting>().HasKey(x => x.MeetingId);
            modelBuilder.Entity<TreeNode>().ToTable("Tree","mt").HasKey(x => x.TreeId);
           // modelBuilder.Entity<Tree>().HasOne(x => x.Parent).WithMany(x => x.Parent);

            base.OnModelCreating(modelBuilder);
        }
    }
}
