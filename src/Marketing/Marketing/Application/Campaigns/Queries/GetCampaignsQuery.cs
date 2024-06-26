﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Identity;
using YourBrand.Marketing.Domain;
using YourBrand.Marketing.Domain.Entities;

namespace YourBrand.Marketing.Application.Campaigns.Queries;

public record GetCampaignsQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<CampaignDto>>
{
    sealed class GetCampaignsQueryHandler(
        IMarketingContext context,
        IUserContext userContext) : IRequestHandler<GetCampaignsQuery, ItemsResult<CampaignDto>>
    {
        public async Task<ItemsResult<CampaignDto>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Campaign> result = context
                    .Campaigns
                    .OrderBy(o => o.Created)
                    .AsNoTracking()
                    .AsQueryable();

            if (request.SearchString is not null)
            {
                result = result.Where(o => o.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await result.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                result = result.OrderBy(request.SortBy, request.SortDirection == Application.Common.Models.SortDirection.Desc ? Marketing.Application.SortDirection.Descending : Marketing.Application.SortDirection.Ascending);
            }
            else
            {
                result = result.OrderBy(x => x.Name);
            }

            var items = await result
                .Skip((request.Page) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<CampaignDto>(items.Select(cp => cp.ToDto()), totalCount);
        }
    }
}