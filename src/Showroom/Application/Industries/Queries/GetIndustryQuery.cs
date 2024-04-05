﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Identity;
using YourBrand.Showroom.Application.Common.Interfaces;

namespace YourBrand.Showroom.Application.Industries.Queries;

public record GetIndustryQuery(int Id) : IRequest<IndustryDto?>
{
    class GetIndustryQueryHandler : IRequestHandler<GetIndustryQuery, IndustryDto?>
    {
        private readonly IShowroomContext _context;
        private readonly IUserContext userContext;

        public GetIndustryQueryHandler(
            IShowroomContext context,
            IUserContext userContext)
        {
            _context = context;
            this.userContext = userContext;
        }

        public async Task<IndustryDto?> Handle(GetIndustryQuery request, CancellationToken cancellationToken)
        {
            var industry = await _context
               .Industries
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id);

            if (industry is null)
            {
                return null;
            }

            return industry.ToDto();
        }
    }
}