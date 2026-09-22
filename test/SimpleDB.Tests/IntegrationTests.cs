using System.Net.Http.Json;
using Model;
using Xunit;

namespace Bison.CLI.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task CanReadObservationsFromWebService()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5256")
        };

        var observations =
            await client.GetFromJsonAsync<IEnumerable<Observation>>(
                "/observations");

        Assert.NotNull(observations);
    }
}