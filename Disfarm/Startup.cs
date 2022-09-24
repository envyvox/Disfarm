using System;
using Disfarm.Data;
using Disfarm.Services.Discord.Client;
using Disfarm.Services.Discord.Extensions;
using Disfarm.Services.Game.Localization;
using Disfarm.Services.Game.Localization.Impl;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteFarmWatering;
using Disfarm.Services.Hangfire.BackgroundJobs.CompleteFishing;
using Disfarm.Services.Hangfire.BackgroundJobs.StartNewDay;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace Disfarm
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordClientOptions>(x => _config.GetSection("DiscordOptions").Bind(x));

            services.AddDbContextPool<AppDbContext>(o =>
            {
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                o.EnableSensitiveDataLogging();
                o.UseNpgsql(_config.GetConnectionString("main"),
                    s => { s.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name); });
            });

            services.AddHangfireServer();
            services.AddHangfire(config =>
            {
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute {Attempts = 0});
                config.UsePostgreSqlStorage(_config.GetConnectionString("main"));
            });

            services.AddAutoMapper(typeof(IDiscordClientService).Assembly);
            services.AddMediatR(typeof(IDiscordClientService).Assembly);
            services.AddMemoryCache();

            services
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            services.AddOpenApiDocument();

            services.AddSingleton(_ =>
                TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("CronTimezoneId")));

            services.AddSingleton<CommandHandler>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddSingleton<ICompleteFishingJob, CompleteFishingJob>();
            services.AddSingleton<ICompleteFarmWateringJob, CompleteFarmWateringJob>();
            services.AddSingleton<IStartNewDayJob, StartNewDayJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDb(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {new AllowAllAuthorizationFilter()}
            });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build());

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.StartDiscord();
        }

        private static void MigrateDb(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }

        private class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
    }
}