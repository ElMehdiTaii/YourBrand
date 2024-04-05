﻿using MediatR;

using YourBrand.Analytics;
using YourBrand.StoreFront.API;

namespace YourBrand.StoreFront.Application.Features.Analytics;

public sealed record CreateClient : IRequest<string>
{
    sealed class Handler : IRequestHandler<CreateClient, string>
    {
        private readonly IClientClient clientClient;
        private readonly IUserContext userContext;

        public Handler(
            YourBrand.Analytics.IClientClient clientClient,
            IUserContext userContext)
        {
            this.clientClient = clientClient;
            this.userContext = userContext;
        }

        public async Task<string> Handle(CreateClient request, CancellationToken cancellationToken)
        {
            var userAgent = userContext.UserAgent!.ToString();

            return await clientClient.InitClientAsync(new YourBrand.Analytics.ClientData()
            {
                UserAgent = userAgent!
            }, cancellationToken);
        }
    }
}