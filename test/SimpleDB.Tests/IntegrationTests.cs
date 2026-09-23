using System.Net.Http.Json;
using Model;
using SimpleDB;
using Xunit;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    //IntegrationTests removed for now to eliminate any need for using WebApplicationFactory<Program>.
    
    /*
    [Fact]
    public async Task CanReadObservationsFromWebService()
    {
        //Arrange
        var client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5256")
        };

        //Act
        var observations =
            await client.GetFromJsonAsync<IEnumerable<Observation>>(
                "/observations");

        //Assert
        Assert.NotNull(observations);
    }

        [Fact]
    public void FirstObservationInDBIsCorrect() {
        //Arrange
        var database = CSVDatabase<Observation>.getInstance(@"..\..\..\..\..\src\Bison.CLI\bison_observe_cli_db.csv");
        var records = database.Read();

        //Act
        var result = records.First().Message == "A bird at DR Byen";

        //Assert
        Assert.True(result);
    }*/
}