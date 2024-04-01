﻿
using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using YourBrand.ApiKeys;
using YourBrand.Showroom.Application.Common.Models;
using YourBrand.Showroom.Application.PersonProfiles;
using YourBrand.Showroom.Application.PersonProfiles.Commands;
using YourBrand.Showroom.Application.PersonProfiles.Experiences;
using YourBrand.Showroom.Application.PersonProfiles.Experiences.Commands;
using YourBrand.Showroom.Application.PersonProfiles.Experiences.Queries;
using YourBrand.Showroom.Application.PersonProfiles.Queries;
using YourBrand.Showroom.Application.PersonProfiles.Search.Queries;
using YourBrand.Showroom.Application.PersonProfiles.Skills.Commands;
using YourBrand.Showroom.Application.PersonProfiles.Skills.Queries;
using YourBrand.Showroom.Domain.Enums;

namespace YourBrand.Showroom.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = AuthSchemes.Default)]
public class PersonProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Results<PersonProfileDto>> GetPersonProfiles(int page = 1, int pageSize = 10, string? organizationId = null, string? competenceAreaId = null, DateTime? availableFrom = null, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetPersonProfilesAsync(page - 1, pageSize, organizationId, competenceAreaId, availableFrom, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpPost("Find")]
    public async Task<Results<PersonProfileDto>> FindPersonProfiles([FromBody] PersonProfileQuery query, int page = 1, int pageSize = 10, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new FindPersonProfilesQuery(query, page - 1, pageSize, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<PersonProfileDto?> GetPersonProfile(string id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetPersonProfileQuery(id), cancellationToken);
    }

    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(typeof(PersonProfileDto), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreatePersonProfile(CreatePersonProfileDto dto, CancellationToken cancellationToken)
    {
        var dto2 = await _mediator.Send(new CreatePersonProfileCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetPersonProfile), new { id = dto2.Id }, dto2);
    }

    [HttpPut("{id}")]
    public async Task UpdatePersonProfile(string id, UpdatePersonProfileDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdatePersonProfileCommand(id, dto), cancellationToken);
    }

    [HttpPut("{id}/Details")]
    public async Task UpdateDetails(string id, [FromBody] PersonProfileDetailsDto details, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateDetailsCommand(id, details), cancellationToken);
    }

    [HttpPut("{id}/Headline")]
    public async Task UpdateHeadline(string id, [FromBody] string text, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateHeadlineCommand(id, text), cancellationToken);
    }

    [HttpPut("{id}/Presentation")]
    public async Task UpdatePresentation(string id, [FromBody] string text, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdatePresentationCommand(id, text), cancellationToken);
    }

    [HttpPut("{id}/Picture")]
    public async Task UpdatePicture(string id, IFormFile file, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UploadImageCommand(id, file.OpenReadStream()), cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeletePersonProfile(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePersonProfileCommand(id), cancellationToken);
    }

    [HttpGet("{id}/Experiences")]
    public async Task<Results<ExperienceDto>> GetExperiences(string? id, int page = 1, int? pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetExperiencesQuery(page - 1, pageSize, id, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}/Experiences/{experienceId}")]
    public async Task<ExperienceDto?> GetExperience(string id, string experienceId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetExperienceQuery(id, experienceId), cancellationToken);
    }

    [HttpPost("{id}/Experiences")]
    public async Task AddExperience(string id, CreateExperienceDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AddExperienceCommand(id, dto.Title, dto.CompanyId, dto.Location, dto.EmploymentType, dto.StartDate, dto.EndDate, dto.Description),
            cancellationToken);
    }

    [HttpPut("{id}/Experiences/{experienceId}")]
    public async Task UpdateExperience(string id, string experienceId, UpdateExperienceDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateExperienceCommand(id, experienceId,
            dto.Title, dto.CompanyId, dto.Location, dto.EmploymentType, dto.StartDate, dto.EndDate, dto.Description),
            cancellationToken);
    }

    [HttpDelete("{id}/Experiences/{experienceId}")]
    public async Task RemoveExperience(string id, string experienceId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveExperienceCommand(id, experienceId), cancellationToken);
    }

    [HttpGet("{id}/Skills")]
    public async Task<Results<PersonProfileSkillDto>> GetSkills(string id, int page = 1, int? pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetSkillsQuery(id, page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}/Skills/{skillId}")]
    public async Task<PersonProfileSkillExperiencesDto?> GetSkill(string id, string skillId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetSkillExperiencesQuery(id, skillId), cancellationToken);
    }

    [HttpPost("{id}/Skills/{skillId}/Experiences")]
    public async Task UpdateSkillExperiences(string id, string skillId, [FromBody] IEnumerable<UpdateSkillExperienceDto> experiences, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateSkillExperiencesCommand(id, skillId, experiences), cancellationToken);
    }

    [HttpPost("{id}/Skills")]
    public async Task<PersonProfileSkillDto> AddSkill(string id, AddPersonProfileSkillDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new AddSkillCommand(id, dto.SkillId, dto.Level, dto.Comment),
            cancellationToken);
    }

    [HttpPut("{id}/Skills/{skillId}")]
    public async Task<PersonProfileSkillDto> UpdateSkill(string id, string skillId, UpdatePersonProfileSkillDto dto, CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new UpdateSkillCommand(skillId, dto.Level, dto.Comment),
            cancellationToken);
    }

    [HttpDelete("{id}/Skills/{skillId}")]
    public async Task RemoveSkill(string id, string skillId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveSkillCommand(skillId), cancellationToken);
    }
}

public record AddPersonProfileSkillDto(string SkillId, SkillLevel Level, string? Comment);

public record UpdatePersonProfileSkillDto(SkillLevel Level, string? Comment);