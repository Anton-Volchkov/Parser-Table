using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkBot.Bot;
using VkBot.Bot.Commands;
using VkBot.Data.Abstractions;

namespace VkBot.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddBotFeatures(this IServiceCollection services)
        {
            services.AddScoped<CommandExecutor>();

            services.AddScoped<IBotCommand,StudentInfo>();
            services.AddScoped<IBotCommand, Help>();
        }
    }
}
