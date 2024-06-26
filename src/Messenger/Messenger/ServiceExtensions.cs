﻿using YourBrand.Identity;

namespace YourBrand.Messenger;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddUserContext();

        return services;
    }
}