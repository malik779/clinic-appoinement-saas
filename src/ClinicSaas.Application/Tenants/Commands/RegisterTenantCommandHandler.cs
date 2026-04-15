using ClinicSaas.Application.Abstractions;
using ClinicSaas.Application.Common;
using ClinicSaas.Domain.Entities.Clinic;
using ClinicSaas.Domain.Entities.Subscription;
using ClinicSaas.Domain.Entities.Website;
using ClinicSaas.Domain.Entities.Identity;

namespace ClinicSaas.Application.Tenants.Commands;

public sealed class RegisterTenantCommandHandler(
    ITenantRepository tenantRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ISubscriptionRepository subscriptionRepository,
    IWebsiteRepository websiteRepository)
{
    public async Task<Result<Guid>> HandleAsync(RegisterTenantCommand command, CancellationToken cancellationToken = default)
    {
        var normalizedSubdomain = command.Subdomain.Trim().ToLowerInvariant();
        var existing = await tenantRepository.GetBySubdomainAsync(normalizedSubdomain, cancellationToken);
        if (existing is not null)
        {
            return Result<Guid>.Failure(["Subdomain is already in use."]);
        }

        var tenant = new Tenant
        {
            Name = command.ClinicName.Trim(),
            Subdomain = normalizedSubdomain,
            ContactEmail = command.AdminEmail.Trim().ToLowerInvariant()
        };

        await tenantRepository.AddAsync(tenant, cancellationToken);

        var adminUser = ApplicationUser.Create(
            command.AdminEmail,
            command.AdminFullName,
            tenant.Id,
            tenant.Subdomain);

        var adminCreateResult = await userRepository.CreateAsync(adminUser, command.AdminPassword, cancellationToken);
        if (!adminCreateResult.IsSuccess)
        {
            return Result<Guid>.Failure(adminCreateResult.Errors.Select(x => x.Message).ToArray());
        }

        await roleRepository.EnsureRolesExistAsync(ApplicationRole.DefaultRoles, cancellationToken);
        var roleAssignResult = await userRepository.AssignRoleAsync(adminUser, ApplicationRole.TenantAdmin, cancellationToken);
        if (!roleAssignResult.IsSuccess)
        {
            return Result<Guid>.Failure(roleAssignResult.Errors.Select(x => x.Message).ToArray());
        }

        var adminRole = await roleRepository.GetByNameAsync(ApplicationRole.TenantAdmin, cancellationToken);
        if (adminRole is null)
        {
            return Result<Guid>.Failure("Tenant admin role is not available.");
        }

        var selectedPlanCode = command.PlanCode.Trim().ToLowerInvariant();
        var selectedPlan = await subscriptionRepository.GetPlanByCodeAsync(selectedPlanCode, cancellationToken);
        if (selectedPlan is null)
        {
            return Result<Guid>.Failure("Selected plan does not exist.");
        }

        var utcNow = DateTimeOffset.UtcNow;
        var subscription = new TenantSubscription
        {
            TenantId = tenant.Id,
            PlanId = selectedPlan.Id,
            StartsAtUtc = utcNow,
            TrialStartsAtUtc = utcNow,
            TrialEndsAtUtc = utcNow.AddDays(14),
            Status = "Trialing",
            FeaturesJson = """{"websiteBuilder":true,"appointments":false,"aiAssistant":false}"""
        };

        await subscriptionRepository.AddSubscriptionAsync(subscription, cancellationToken);

        var website = new Website
        {
            TenantId = tenant.Id,
            Theme = "default-light"
        };
        var page = new Page
        {
            TenantId = tenant.Id,
            WebsiteId = website.Id,
            Slug = "home",
            Title = "Home"
        };
        page.Sections.Add(new Section
        {
            TenantId = tenant.Id,
            PageId = page.Id,
            Type = "hero",
            SortOrder = 1,
            ContentJson = """{"title":"Welcome","subtitle":"Clinic care made simple","ctaText":"Book now"}"""
        });
        website.Pages.Add(page);

        await websiteRepository.AddAsync(website, cancellationToken);

        return Result<Guid>.Success(tenant.Id);
    }
}
