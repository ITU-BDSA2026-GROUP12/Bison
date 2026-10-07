using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Bison.Razor.Tests;

public class BisonRazorPagesIntegratonTest : IClassFixture<WebApplicationFactory<Program>> {

    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public BisonRazorPagesIntegratonTest(WebApplicationFactory<Program> factory, ITestOutputHelper output) {
        _client = factory.CreateClient();   //Injected by WebApplicationFactory
        _output = output;   //Injected by ITestOutputHelper. Used for debugging. Will only output stuff if a test fails.
    }

    [Fact]
    public async Task PeterFoundABigBirdNoPageTest() {

        // no Arrange

        // Act
        var response = await _client.GetAsync("/ob");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        _output.WriteLine(html);

        // Assert
        //TODO: Is this enough? Peter might have made another observation and another person might have observed "a big bird".
        Assert.DoesNotContain("Peter", html);
        Assert.DoesNotContain("a big bird", html.ToLower());
    }

    [Fact]
    public async Task EduardFoundAHeronNoPageTest() {

        // no Arrange

        // Act
        var response = await _client.GetAsync("/Eduard");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        _output.WriteLine(html);

        // Assert
        Assert.DoesNotContain("a heron", html.ToLower());
    }
}