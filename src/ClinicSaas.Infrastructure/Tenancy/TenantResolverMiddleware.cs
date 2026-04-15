using System.Security.Claims;
using ClinicSaas.Application.Abstractions;
using ClinicSaas.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicSaas.Infrastructure.Tenancy;

public sealed class TenantResolverMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, ApplicationDbContext dbContext)
    {
        var tenantResolution = await ResolveTenantAsync(context, dbContext);
        if (tenantResolution is { })
        {
            tenantContext.TenantId = tenantResolution.Value.Id;
            tenantContext.TenantSubdomain = tenantResolution.Value.Subdomain;
        }

        var tenantFromClaim = context.User.FindFirstValue(TenantConstants.TenantIdClaimType);
        if (Guid.TryParse(tenantFromClaim, out var claimTenantId))
        {
            if (tenantContext.TenantId.HasValue && tenantContext.TenantId.Value != claimTenantId)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    data = (object?)null,
                    errors = new[] { "Tenant mismatch between token and request." }
                });
                return;
            }

            tenantContext.TenantId = claimTenantId;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var currentUserContext = context.RequestServices.GetRequiredService<ICurrentUserContext>() as CurrentUserContext;
            if (currentUserContext is not null)
            {
                var userIdValue = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                                  ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                currentUserContext.UserId = Guid.TryParse(userIdValue, out var userId) ? userId : null;
                currentUserContext.Email = context.User.FindFirstValue(ClaimTypes.Email)
                                           ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Email);
                currentUserContext.Roles = context.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
            }
        }

        await next(context);
    }

    private static async Task<(Guid Id, string Subdomain)?> ResolveTenantAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        if (context.Request.Headers.TryGetValue(TenantConstants.TenantHeader, out var tenantHeader) &&
            Guid.TryParse(tenantHeader.ToString(), out var tenantIdFromHeader))
        {
            var tenantById = await dbContext.Tenants.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tenantIdFromHeader, context.RequestAborted);
            if (tenantById is not null)
            {
                return (tenantById.Id, tenantById.Subdomain);
            }
        }

        var host = context.Request.Host.Host;
        if (string.IsNullOrWhiteSpace(host))
        {
            return null;
        }

        var segments = host.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments.Length < 3)
        {
            return null;
        }

        var subdomain = segments[0].ToLowerInvariant();
        var tenantBySubdomain = await dbContext.Tenants.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Subdomain == subdomain, context.RequestAborted);
        return tenantBySubdomain is null ? null : (tenantBySubdomain.Id, tenantBySubdomain.Subdomain);
    }
}
