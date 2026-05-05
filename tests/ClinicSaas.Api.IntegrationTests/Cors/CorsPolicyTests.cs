using System.Net;
using FluentAssertions;

namespace ClinicSaas.Api.IntegrationTests.Cors;

public sealed class CorsPolicyTests : IClassFixture<Support.ClinicApiFactory>
{
    private const string FrontendOrigin = "http://localhost:5173";
    private readonly HttpClient _client;

    public CorsPolicyTests(Support.ClinicApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PreflightRequest_ReturnsCorsHeaders_ForConfiguredFrontendOrigin()
    {
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/auth/login");
        request.Headers.Add("Origin", FrontendOrigin);
        request.Headers.Add("Access-Control-Request-Method", "POST");
        request.Headers.Add("Access-Control-Request-Headers", "content-type,x-tenant-id,authorization");

        using var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Headers.TryGetValues("Access-Control-Allow-Origin", out var allowedOrigins).Should().BeTrue();
        allowedOrigins.Should().ContainSingle().Which.Should().Be(FrontendOrigin);
        response.Headers.TryGetValues("Access-Control-Allow-Methods", out var allowedMethods).Should().BeTrue();
        allowedMethods.Should().Contain(methods => methods.Contains("POST", StringComparison.OrdinalIgnoreCase));
        response.Headers.TryGetValues("Access-Control-Allow-Headers", out var allowedHeaders).Should().BeTrue();
        allowedHeaders.Should().Contain(headers => headers.Contains("authorization", StringComparison.OrdinalIgnoreCase));
    }
}
