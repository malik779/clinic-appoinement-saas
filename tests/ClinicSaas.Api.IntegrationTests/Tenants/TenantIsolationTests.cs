using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace ClinicSaas.Api.IntegrationTests.Tenants;

public sealed class TenantIsolationTests : IClassFixture<Support.ClinicApiFactory>
{
    private readonly HttpClient _client;

    public TenantIsolationTests(Support.ClinicApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterUser_ReturnsBadRequest_WhenTenantHeaderIsMissing()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            email = "staff@missingtenant.com",
            password = "Password@123",
            fullName = "Staff User",
            role = "Staff"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
