using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SalaRemota.Api.IntegrationTests;

public sealed class AdversarialEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly string[] ForbiddenResponseFragments =
    {
        "connection string",
        "exception",
        "npgsql",
        "stack trace",
        "C:\\Users\\",
        "Marcos"
    };

    private readonly HttpClient _client;

    public AdversarialEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://sala-remota.example")
        });
    }

    [Fact]
    public async Task Get_unknown_api_route_returns_safe_not_found()
    {
        var response = await _client.GetAsync("/api/v1/nao-existe");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await AssertResponseHasNoInternalDetails(response);
    }

    [Fact]
    public async Task Post_health_is_not_supported_and_returns_safe_response()
    {
        using var content = new StringContent("{}");

        var response = await _client.PostAsync("/api/v1/health", content);

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        await AssertResponseHasNoInternalDetails(response);
    }

    [Fact]
    public async Task Get_health_ignores_unexpected_query_and_header_safely()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/v1/health?unexpected=%3Cscript%3Ealert%281%29%3C%2Fscript%3E");
        request.Headers.Add("X-Unusual-Header", "unexpected-value");

        var response = await _client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"status\":\"Healthy\"}", body);
        Assert.DoesNotContain("script", body, StringComparison.OrdinalIgnoreCase);
        await AssertResponseHasNoInternalDetails(response);
    }

    [Fact]
    public async Task Get_unexpected_path_does_not_expose_internal_details()
    {
        var response = await _client.GetAsync("/api/v1/%2e%2e/%2e%2e/unexpected");

        Assert.False(response.IsSuccessStatusCode);
        await AssertResponseHasNoInternalDetails(response);
    }

    private static async Task AssertResponseHasNoInternalDetails(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();

        foreach (var fragment in ForbiddenResponseFragments)
        {
            Assert.DoesNotContain(fragment, body, StringComparison.OrdinalIgnoreCase);
        }
    }
}
