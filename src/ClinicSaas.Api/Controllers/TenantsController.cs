using ClinicSaas.Api.Contracts;
using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Tenants.Commands;
using ClinicSaas.Application.Tenants.Queries;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSaas.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tenants")]
public sealed class TenantsController(
    RegisterTenantCommandHandler registerTenantHandler,
    ITenantContext tenantContext,
    GetTenantInfoQueryHandler getTenantInfoHandler) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterTenantRequest request, CancellationToken cancellationToken)
    {
        var result = await registerTenantHandler.HandleAsync(
            new RegisterTenantCommand(
                request.ClinicName,
                request.Subdomain,
                request.AdminEmail,
                request.AdminFullName,
                request.AdminPassword,
                request.PlanCode),
            cancellationToken);

        return result.IsSuccess
            ? Ok(ApiResponseFactory.Success(new RegisterTenantResponse(result.Value)))
            : BadRequest(ApiResponseFactory.Failure(result.Errors.Select(x => x.Message).ToArray()));
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyTenant(CancellationToken cancellationToken)
    {
        if (!tenantContext.TenantId.HasValue)
        {
            return BadRequest(ApiResponseFactory.Failure("Tenant context is required."));
        }

        var result = await getTenantInfoHandler.HandleAsync(new GetTenantInfoQuery(tenantContext.TenantId.Value), cancellationToken);
        return result.IsSuccess
            ? Ok(ApiResponseFactory.Success(result.Value))
            : BadRequest(ApiResponseFactory.Failure(result.Errors.Select(x => x.Message).ToArray()));
    }
}

public sealed record RegisterTenantRequest(
    string ClinicName,
    string Subdomain,
    string AdminEmail,
    string AdminFullName,
    string AdminPassword,
    string PlanCode);

public sealed record RegisterTenantResponse(Guid TenantId);
