﻿namespace YourBrand.Documents.Application.Common.Models;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);