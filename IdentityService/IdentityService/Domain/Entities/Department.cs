﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace YourBrand.IdentityService.Domain.Entities;

public class Department 
{
    public string Id { get; set; }

    public string Name { get; set; }

    public Organization Organization { get; set; }

    public List<User> Users { get; } = new List<User>();
}
