﻿using Microsoft.Extensions.DependencyInjection;

using YourBrand.HumanResources.Domain.Entities;

namespace YourBrand.HumanResources.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            /*
            var organization = new Organization("my-company", "My Company") {
                Currency = "SEK"
            };

            context.Organizations.Add(organization);

            var person = new Person(organization, "Test", "Testsson", null, "Software Developer", "19900105-3835", "test@test.com");
            
            context.Persons.Add(person);
            */

            context.Roles.Add(new Role("User"));
            context.Roles.Add(new Role("Administrator"));

            await context.SaveChangesAsync();
        }
    }
}