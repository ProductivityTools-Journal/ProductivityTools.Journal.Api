using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using ProductivityTools.CreateSQLServerDatabase;
using ProductivityTools.Meetings.DatabaseMigrations.Migrations;
using System;

namespace ProductivityTools.Meetings.DatabaseMigrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database("PTMeetings", "Server=.\\SQL2019;Trusted_Connection=True;");
            database.CreateSilent();

            var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }


            Console.WriteLine("Hello World!");
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString("Server=.\\sql2019;Database=PTMeetings;Integrated Security=True")
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateMeetingsTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}
