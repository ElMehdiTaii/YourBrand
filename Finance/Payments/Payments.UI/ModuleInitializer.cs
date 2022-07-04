﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal.Shared;
using YourBrand.Payments.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.Navigation;

namespace YourBrand.Payments;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddPaymentsClients((sp, httpClient) => {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{navigationManager.BaseUri}api/payments/");
        }, builder => {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        /*
        var navManager = services
            .GetRequiredService<NavManager>();

        var group = navManager.GetGroup("finance") ?? navManager.CreateGroup("finance", "Finance");
        group.CreateItem("payments", "Payments", MudBlazor.Icons.Material.Filled.List, "/payments");
        */
    }
}