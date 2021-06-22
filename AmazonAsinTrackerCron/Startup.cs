using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AmazonAsinTrackerCron.Jobs;
using AmazonAsinTrackerCron.ServicesExtension;

namespace AmazonAsinTrackerCron
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronJob<CronJobService>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                // "* * * * *" ==> run every minutes cf https://github.com/HangfireIO/Cronos
                c.CronExpression = @"* * * * *";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
