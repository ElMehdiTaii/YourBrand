﻿
using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Application.Setup;

namespace YourBrand.WebApi.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class SetupController : Controller
{
    private readonly IMediator _mediator;

    public SetupController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Setup(SetupRequest request, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetupCommand(request.OrganizationName, request.Email, request.Password), cancellationToken);
        return Ok();
    }
}


public record SetupRequest(string OrganizationName, string Email, string Password);