using System.Security.Claims;
using Asp.Versioning;
using ClinicSaas.Api.Contracts;
using ClinicSaas.Application.Tenants.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSaas.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tenant")]
[Authorize]
public sealed class TenantInfoController(GetTenantInfoQueryHandler getTenantInfoQueryHandler) : ControllerBase
{
    [HttpGet("info")]
    public async Task<IActionResult> GetInfo(CancellationToken cancellationToken)
    {
        var tenantIdValue = User.FindFirstValue("tenantId");
        if (!Guid.TryParse(tenantIdValue, out var tenantId))
        {
            return Unauthorized(ApiResponseFactory.Failure("Missing tenant claim."));
        }

        var result = await getTenantInfoQueryHandler.HandleAsync(new GetTenantInfoQuery(tenantId), cancellationToken);
        return result.IsSuccess
            ? Ok(ApiResponseFactory.Success(result.Value))
            : NotFound(ApiResponseFactory.Failure(result.Errors.Select(x => x.Message).ToArray()));
    }
}
