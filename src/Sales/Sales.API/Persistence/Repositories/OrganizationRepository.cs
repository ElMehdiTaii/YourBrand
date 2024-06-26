using Microsoft.EntityFrameworkCore;

using YourBrand.Domain;
using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Domain.Specifications;
using YourBrand.Sales.Features.OrderManagement.Repositories;

namespace YourBrand.Sales.Persistence.Repositories.Mocks;

public sealed class OrganizationRepository(SalesContext context) : IOrganizationRepository
{
    readonly DbSet<Organization> dbSet = context.Set<Organization>();

    public IQueryable<Organization> GetAll()
    {
        //return dbSet.Where(new OrganizationsWithStatus(OrganizationStatus.Completed).Or(new OrganizationsWithStatus(OrganizationStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Organization?> FindByIdAsync(OrganizationId id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IQueryable<Organization> GetAll(ISpecification<Organization> specification)
    {
        return dbSet
            .Include(x => x.Users).Where(specification.Criteria);
    }

    public void Add(Organization organization)
    {
        dbSet.Add(organization);
    }

    public void Remove(Organization organization)
    {
        dbSet.Remove(organization);
    }
}