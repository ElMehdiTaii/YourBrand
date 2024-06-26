﻿namespace YourBrand.HumanResources.Contracts;

public record CreateOrganization
{
    public string Name { get; init; }
    public string? FriendlyName { get; init; }
}

public record CreateOrganizationResponse(string Id, string Name, string? FriendlyName);

public record OrganizationCreated(string OrganizationId, string Name);

public record OrganizationUpdated(string OrganizationId, string Name);

public record OrganizationDeleted(string OrganizationId);

public record GetOrganizationResponse(string Id, string Name, string? FriendlyName);