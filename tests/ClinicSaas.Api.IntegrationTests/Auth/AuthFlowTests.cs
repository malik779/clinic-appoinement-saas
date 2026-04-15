using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace ClinicSaas.Api.IntegrationTests.Auth;

public sealed class AuthFlowTests : IClassFixture<Support.ClinicApiFactory>
{
    private readonly Support.ClinicApiFactory _factory;

    public AuthFlowTests(Support.ClinicApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenTenantHeaderIsMissing()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "admin@clinic.com", password = "Password@123" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RegisterAndLogin_ReturnsJwt_WhenTenantHeaderIsValid()
    {
        var tenantClient = _factory.CreateClient();

        var registerTenantResponse = await tenantClient.PostAsJsonAsync(
            "/api/v1/tenants/register",
            new
            {
                clinicName = "Blue Clinic",
                subdomain = "blue-clinic",
                adminEmail = "admin@blueclinic.com",
                adminFullName = "Blue Owner",
                adminPassword = "Password@123",
                planCode = "starter"
            });
        if (registerTenantResponse.StatusCode != HttpStatusCode.OK)
        {
            var registerTenantBody = await registerTenantResponse.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException($"Tenant registration failed with {(int)registerTenantResponse.StatusCode}: {registerTenantBody}");
        }
        registerTenantResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tenantPayload = await registerTenantResponse.Content
            .ReadFromJsonAsync<ApiEnvelope<RegisterTenantData>>();
        tenantPayload.Should().NotBeNull();
        tenantPayload!.Data.Should().NotBeNull();

        var tenantId = tenantPayload.Data!.TenantId;

        var authClient = _factory.CreateClient();
        authClient.DefaultRequestHeaders.Add("X-Tenant-Id", tenantId.ToString());

        var loginResponse = await authClient.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "admin@blueclinic.com", password = "Password@123" });

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<ApiEnvelope<AuthData>>();
        loginPayload.Should().NotBeNull();
        loginPayload!.Data.Should().NotBeNull();
        loginPayload.Data!.Tokens.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    private sealed record ApiEnvelope<T>(bool Success, T? Data, string[] Errors);
    private sealed record RegisterTenantData(Guid TenantId);
    private sealed record AuthData(AuthTokens Tokens);
    private sealed record AuthTokens(string AccessToken, DateTimeOffset ExpiresAtUtc);
}
