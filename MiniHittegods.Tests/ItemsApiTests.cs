using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MiniHittegods.Tests;

public class ItemsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ItemsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetItemsWithInvalidStatusReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/items?status=InvalidStatus");
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetItemsReturnsOk()
    {
        var response = await _client.GetAsync("/api/items");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}