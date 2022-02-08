using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Services
{
    public static class Services
    {
        public static IServiceCollection ConfigureServicesTreeService(this IServiceCollection services)
        {
            services.AddScoped<ITreeService, TreeService>();
            services.AddScoped<IMeetingService, MeetingService>();
            return services;
        }
    }
}
