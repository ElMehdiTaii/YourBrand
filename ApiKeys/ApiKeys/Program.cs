﻿
using YourBrand.ApiKeys;
using YourBrand.ApiKeys.Application;
using YourBrand.ApiKeys.Authentication;
using YourBrand.ApiKeys.Infrastructure;
using YourBrand.ApiKeys.Infrastructure.Persistence;

using MassTransit;

using NSwag;
using NSwag.Generation.Processors.Security;

using Serilog;

using YourBrand;
using YourBrand.Extensions;

static class Program
{
    /// <param name="seed">Seed the database</param>
    /// <param name="args">The rest of the arguments</param>
    /// <returns></returns>
    static async Task Main(bool seed, string? connectionString, string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string ServiceName =  "ApiKeys";
        string ServiceVersion = "1.0";

        // Add services to container

        builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration)
                                .Enrich.WithProperty("Application", ServiceName)
                                .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName));

        builder.Services
            .AddOpenApi(ServiceName, ApiVersions.All, settings =>
            {
                settings
                    .AddApiKeySecurity()
                    .AddJwtSecurity();
            })
            .AddApiVersioningServices();

        builder.Services.AddObservability(ServiceName, ServiceVersion, builder.Configuration);

        builder.Services.AddProblemDetails();

        if (connectionString is not null)
        {
            builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
        }

        var Configuration = builder.Configuration;

        var services = builder.Services;

        services.AddApplication(Configuration);
        services.AddInfrastructure(Configuration);
        services.AddServices();

        services
            .AddControllers()
            .AddNewtonsoftJson();

        builder.Services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        services.AddAuthWithJwt();
        services.AddAuthWithApiKey();

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(typeof(Program).Assembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        app.MapObservability();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseOpenApiAndSwaggerUi();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!");

        app.MapControllers();

        if (seed)
        {
            await app.Services.SeedAsync();
            return;
        }

        app.Run();
    }
}