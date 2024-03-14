﻿using Microsoft.Extensions.DependencyInjection;

using YourBrand.TimeReport;
using YourBrand.HumanResources;
using YourBrand.HumanResources.Client;

const string ApiKey = "asdsr34#34rswert35234aedae?2!";

var services = BuildServiceProvider();

var organizationsClient = services.GetRequiredService<IOrganizationsClient>();
var personsClient = services.GetRequiredService<IPersonsClient>();
var syncClient = services.GetRequiredService<ISyncClient>();

if (args.ToArray().Contains("--sync-users"))
{
    await syncClient.SyncDataAsync();
    return;
}

static IServiceProvider BuildServiceProvider()
{
    ServiceCollection services = new();

    services.AddHumanResourcesClients((sp, http) =>
    {
        http.BaseAddress = new Uri($"https://localhost:5174/api/humanresources/");
        //http.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
    }, (builder) => { });

    services.AddTimeReportClients((sp, http) =>
    {
        http.BaseAddress = new Uri($"https://localhost:5174/api/timereport/");
        http.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
    }, (builder) => { });

    return services.BuildServiceProvider();
}