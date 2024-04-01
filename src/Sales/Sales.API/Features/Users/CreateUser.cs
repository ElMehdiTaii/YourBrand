using FluentValidation;

using MediatR;

using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Features.OrderManagement.Orders;
using YourBrand.Sales.Features.OrderManagement.Repositories;

namespace YourBrand.Sales.Features.OrderManagement.Users;

public record CreateUser(string Name, string Email, string? TenantId, string? UserId = null) : IRequest<Result<UserInfoDto>>
{
    public class Validator : AbstractValidator<CreateUser>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<CreateUser, Result<UserInfoDto>>
    {
        public async Task<Result<UserInfoDto>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(request.UserId!, cancellationToken);

            if (user is not null)
            {
                return Result.Success(user.ToDto2());
            }

            string userId = request.UserId ?? currentUserService.UserId!;

            userRepository.Add(new User(userId, request.Name, request.Email)
            {
                TenantId = request.TenantId
            });

            await unitOfWork.SaveChangesAsync(cancellationToken);

            user = await userRepository.FindByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserInfoDto>(Errors.Users.UserNotFound);
            }

            return Result.Success(user.ToDto2());
        }
    }
}