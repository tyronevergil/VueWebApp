using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SimpleBus;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddMvc();
            services.AddSignalR();

            services.AddSingleton<ITodoContextFactory, TodoContextFactory>();
            services.AddSingleton<IMessageContextFactory, MessageContextFactory>();
            services.AddSingleton<IMessageStore, MessageStore>();
            services.AddSingleton<IMessageHandler, TaskTypeCreateHandler>();
            services.AddSingleton<IMessageHandler, TaskCreateHandler>();
            services.AddSingleton<IMessageHandler, TaskCompleteHandler>();
            services.AddSingleton<IMessageHandler, TaskTypeCreatedHandler>();
            services.AddSingleton<IMessageHandler, TaskCreatedHandler>();
            services.AddSingleton<IMessageHandler, TaskCompletedHandler>();
            services.AddSingleton<IServiceBus, ServiceBus>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes => {
                routes.MapHub<DataHub>("/dataHub");
            });
        }
    }
}
