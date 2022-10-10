using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using YourBrand.Showroom.Application.Common.Interfaces;
using YourBrand.Showroom.Application.ConsultantProfiles;
using YourBrand.Showroom.Application.CompetenceAreas;
using YourBrand.Showroom.Application.Users;
using YourBrand.Showroom.Application.Common.Models;
using YourBrand.Showroom.Application.Organizations;
using YourBrand.Showroom.Application.ConsultantProfiles.Experiences;
using YourBrand.Showroom.Application.Skills;
using YourBrand.Showroom.Application.ConsultantProfiles.Skills.Queries;
using YourBrand.Showroom.Application.Cases;
using YourBrand.Showroom.Application.Companies;
using YourBrand.Showroom.Application.Industries;

namespace YourBrand.Showroom.Application;

public static class Mapper
{
    public static UserDto ToDto(this Domain.Entities.User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName, user.SSN, user.Email, user.Created, user.Deleted);
    }

    public static ConsultantProfileDto ToDto(this Domain.Entities.ConsultantProfile consultantProfile, IUrlHelper urlHelper)
    {
        return new ConsultantProfileDto(
            consultantProfile.Id,
            consultantProfile.FirstName,
            consultantProfile.LastName,
            consultantProfile.DisplayName,
            consultantProfile.BirthDate,
            consultantProfile.Location,
            consultantProfile.Industry.ToDto(),
            consultantProfile.Organization.ToDto(),
            consultantProfile.CompetenceArea.ToDto(),
            urlHelper.CreateImageUrl(consultantProfile.ProfileImage),
            consultantProfile.Headline,
            consultantProfile.ShortPresentation,
            consultantProfile.Presentation,
            consultantProfile.ProfileVideo,
            consultantProfile.AvailableFromDate,
            consultantProfile.Email,
            consultantProfile.PhoneNumber);
    }

    public static CompetenceAreaDto ToDto(this Domain.Entities.CompetenceArea competenceArea)
    {
        return new CompetenceAreaDto(competenceArea.Id, competenceArea.Name);
    }

    public static OrganizationDto ToDto(this Domain.Entities.Organization organization)
    {
        // organization.Id, organization.Name, competenceArea.Created, competenceArea.CreatedBy?.ToDto(), competenceArea.LastModified, competenceArea.LastModifiedBy?.ToDto(

        return new OrganizationDto(organization.Id, organization.Name, organization.Address.ToDto());
    }

    public static AddressDto ToDto(this Domain.ValueObjects.Address address)
    {
        return new AddressDto(address.Address1, address.Address2, address.PostalCode, address.Locality, address.SubAdminArea, address.AdminArea, address.Country);
    }

    public static ExperienceDto ToDto(this Domain.Entities.ConsultantProfileExperience experience)
    {
        return new ExperienceDto(experience.Id, experience.Employment?.Employer?.Name, experience.Title, experience.Company.ToDto(), experience.Location, experience.EmploymentType, experience.StartDate, experience.EndDate, experience.Current, experience.Highlight, experience.Description, 
            experience.Skills.OrderBy(s => s.ConsultantProfileSkill.Skill.Name).Select(x => x.ConsultantProfileSkill.ToDto()));
    }

    public static SkillDto ToDto(this Domain.Entities.Skill skill)
    {
        return new SkillDto(skill.Id, skill.Name, skill.Area.ToDto());
    }

    public static SkillAreaDto ToDto(this Domain.Entities.SkillArea skillArea)
    {
        return new SkillAreaDto(skillArea.Id, skillArea.Name, skillArea.Industry.ToDto());
    }

    public static ConsultantProfileSkillDto ToDto(this Domain.Entities.ConsultantProfileSkill consultantProfileSkill)
    {
        return new ConsultantProfileSkillDto(consultantProfileSkill.Id, consultantProfileSkill.Skill.ToDto(), consultantProfileSkill.Level, consultantProfileSkill.Comment, consultantProfileSkill.Link?.ToDto());
    }

    public static LinkDto ToDto(this Domain.ValueObjects.Link link)
    {
        return new LinkDto(link.Title, link.Href);
    }

    public static CaseDto ToDto(this Domain.Entities.Case @case, IUrlHelper urlHelper)
    {
        return new CaseDto(@case.Id, @case.Status.ToString(), @case.Description, @case.Consultants.Select(x => x.ToDto(urlHelper)));
    }

    public static CaseConsultantDto ToDto(this Domain.Entities.CaseConsultant caseConsultant, IUrlHelper urlHelper)
    {
        return new CaseConsultantDto(caseConsultant.Id, caseConsultant.ConsultantProfile.ToDto(urlHelper), caseConsultant.Presentation);
    }
    
    public static CompanyDto ToDto(this Domain.Entities.Company company)
    {
        return new CompanyDto(company.Id, company.Name, company.Logo, company.Link, company.Industry.ToDto());
    }

    public static IndustryDto ToDto(this Domain.Entities.Industry industry)
    {
        return new IndustryDto(industry.Id, industry.Name);
    }
}