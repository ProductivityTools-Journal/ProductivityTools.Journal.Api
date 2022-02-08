using Microsoft.Extensions.DependencyInjection;
using ProductivityTools.Meetings.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProducvitityTools.Meetings.Queries
{
    public static class Services
    {
        public static IServiceCollection ConfigureServicesQueries(this IServiceCollection services)
        {
            services.AddScoped<IMeetingQueries, MeetingQueries>();
            services.AddScoped<ITreeQueries, TreeQueries>();
            services.ConfigureServicesDatabase();
            return services;
        }
    }
}
