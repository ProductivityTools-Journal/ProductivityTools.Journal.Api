using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.Meetings.Database;

namespace ProductivityTools.Journal.Database.Tests
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

                //services.AddControllers();
                //services.ConfigureServicesTreeService();
                //services.ConfigureServicesQueries();
                //services.ConfigureServicesCommands();
                //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                return services.BuildServiceProvider();
            }
        }


        [TestMethod]
        public void TestMethod1()
        {

            MeetingContext context = new MeetingContext(Configuration);
            var r1=DatabaseHelpers.ExecutVerifyOwnership(context,"pwujczyk@gmail.com", new int[] { 130 });
            Assert.IsTrue(r1);
            var r2 = DatabaseHelpers.ExecutVerifyOwnership(context, "pwujczyk@gmail.com", new int[] { 130,9 });
            Assert.IsFalse(r2);
        }
    }
}