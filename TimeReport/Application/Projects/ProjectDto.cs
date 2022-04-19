﻿using YourBrand.TimeReport.Application.Organizations;

namespace YourBrand.TimeReport.Application.Projects;

public record class ProjectDto(string Id, string Name, string? Description, OrganizationDto Organization);