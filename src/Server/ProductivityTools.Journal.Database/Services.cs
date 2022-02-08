using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Database
{
    public static class Services
    {
        public static IServiceCollection ConfigureServicesDatabase(this IServiceCollection services)
        {
            services.AddScoped<MeetingContext>();
            return services;
        }
    }
}
