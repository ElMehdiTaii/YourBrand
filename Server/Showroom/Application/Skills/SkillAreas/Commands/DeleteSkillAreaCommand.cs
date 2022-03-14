﻿using MediatR;

using Microsoft.EntityFrameworkCore;
using Skynet.Showroom.Application.Common.Interfaces;

namespace Skynet.Showroom.Application.Skills.SkillAreas.Commands;

public record DeleteSkillAreaCommand(string Id) : IRequest
{
    public class DeleteSkillAreaCommandHandler : IRequestHandler<DeleteSkillAreaCommand>
    {
        private readonly IShowroomContext context;

        public DeleteSkillAreaCommandHandler(IShowroomContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteSkillAreaCommand request, CancellationToken cancellationToken)
        {
            var skillArea = await context.SkillAreas
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (skillArea is null) throw new Exception();

            context.SkillAreas.Remove(skillArea);
           
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}