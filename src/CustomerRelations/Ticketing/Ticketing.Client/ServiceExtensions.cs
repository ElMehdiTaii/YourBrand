﻿using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Ticketing.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddTicketingClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddTicketsClient(configureClient, builder)
            .AddTicketStatusesClient(configureClient, builder)
            .AddUsersClient(configureClient, builder);

        return services;
    }
    public static IServiceCollection AddTicketsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(TicketsClient) + "TS", configureClient)
            .AddTypedClient<ITicketsClient>((http, sp) => new TicketsClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddTicketStatusesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(TicketStatusesClient) + "TS", configureClient)
            .AddTypedClient<ITicketStatusesClient>((http, sp) => new TicketStatusesClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddUsersClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(UsersClient) + "TS", configureClient)
            .AddTypedClient<IUsersClient>((http, sp) => new UsersClient(http));

        builder?.Invoke(b);

        return services;
    }
}