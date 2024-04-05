﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Identity;
using YourBrand.Showroom.Application.Common.Interfaces;

namespace YourBrand.Showroom.Application.Skills.Queries;

public record GetSkillQuery(string Id) : IRequest<SkillDto?>
{
    class GetSkillQueryHandler : IRequestHandler<GetSkillQuery, SkillDto?>
    {
        private readonly IShowroomContext _context;
        private readonly IUserContext userContext;

        public GetSkillQueryHandler(
            IShowroomContext context,
            IUserContext userContext)
        {
            _context = context;
            this.userContext = userContext;
        }

        public async Task<SkillDto?> Handle(GetSkillQuery request, CancellationToken cancellationToken)
        {
            var skill = await _context
               .Skills
               .Include(x => x.Area)
               .ThenInclude(x => x.Industry)
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id, cancellationToken);

            if (skill is null)
            {
                return null;
            }

            return skill.ToDto();
        }
    }
}