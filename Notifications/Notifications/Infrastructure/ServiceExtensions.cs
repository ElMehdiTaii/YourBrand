﻿using YourBrand.Notifications.Application.Common.Interfaces;
using YourBrand.Notifications.Infrastructure.Persistence;
using YourBrand.Notifications.Infrastructure.Persistence.Interceptors;
using YourBrand.Notifications.Infrastructure.Services;
using Quartz;
using YourBrand.Notifications.Infrastructure.BackgroundJobs;
using MediatR;
using YourBrand.Notifications.Infrastructure.Idempotence;
using Scrutor;

namespace YourBrand.Notifications.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(trigger => trigger.ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule
                            .WithIntervalInSeconds(10)
                            .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<WorkerContext>(
            configuration.GetConnectionString("mssql", "Worker") ?? configuration.GetConnectionString("DefaultConnection"),
        options => options.EnableRetryOnFailure());

        services.AddScoped<IWorkerContext>(sp => sp.GetRequiredService<WorkerContext>());

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        try 
        {
            services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
        }
        catch(DecorationException exc) when (exc.Message.Contains("Could not find any registered services for type"))
        {
            Console.WriteLine(exc);
        }

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}