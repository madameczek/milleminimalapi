using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using milletest.App.Infrastructure.Api.GetDishes;

namespace milletest.IntegrationTests;

public class GetDishesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ApiVersion = "v1";
    private const string RequestUri = $"/api/{ApiVersion}/dishes";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public GetDishesEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_Return_Not_Empty_Data()
    {
        var httpClient = _factory.CreateClient();

        var response = await httpClient.GetAsync(RequestUri);

        response.EnsureSuccessStatusCode();
        response.Should().NotBeNull();
        var data = await JsonSerializer.DeserializeAsync<IEnumerable<ResponseDto>>(
            await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions);

        var responseDtos = data as ResponseDto[] ?? data!.ToArray();
        responseDtos.Should().NotBeEmpty();
        responseDtos.First().Name.Should().NotBeNull();
    }
}