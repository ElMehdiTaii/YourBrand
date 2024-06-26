﻿using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Documents.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddDocumentsClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddDirectoriesClient(configureClient, builder)
            .AddDocumentsClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddDirectoriesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(DirectoriesClient), configureClient)
            .AddTypedClient<IDirectoriesClient>((http, sp) => new DirectoriesClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddDocumentsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(DocumentsClient), configureClient)
            .AddTypedClient<IDocumentsClient>((http, sp) => new DocumentsClient(http));

        builder?.Invoke(b);

        return services;
    }
}