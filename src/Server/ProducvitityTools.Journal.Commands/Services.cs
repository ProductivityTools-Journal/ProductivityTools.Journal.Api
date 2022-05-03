using Microsoft.Extensions.DependencyInjection;
using ProductivityTools.Meetings.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProducvitityTools.Meetings.Commands
{
    public static class Services
    {
        public static IServiceCollection ConfigureServicesCommands(this IServiceCollection services)
        {
            services.AddScoped<IMeetingCommands, MeetingCommands>();
            services.AddScoped<ITreeCommands, TreeCommands>();
            services.AddScoped<IPermissionCommands, PermissionCommands>();
            services.ConfigureServicesDatabase();
            return services;
        }
    }
}
