using System.Net.Http.Json;
using Model;

public class CommentTest
{
    [Fact]
    public async Task FuzzComments()
    {
        //set up client for communication
        var client = new HttpClient();
        //our server that we'll communicate with
        client.BaseAddress = new Uri("http://localhost:5256");

        var random = new Random();
        var expectedComments = new List<CommentRequest>();

        //get existing observations to ensure valid ObservationIds
        var observations = await client.GetFromJsonAsync<List<Observation>>("/observations");

        //Generate 10 random comments
        for (int i = 0; i < 10; i++)
        {
            //select random excisting observation
            var randomObservation = observations![random.Next(observations.Count)];

            var comment = new CommentRequest(
                ObservationId: randomObservation.ObservationId,
                Author: $"User{random.Next(1, 1000)}",
                Message: $"Random message{random.Next(1, 1000)}",
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            //send comment
            var response = await client.PostAsJsonAsync("/comment", comment);
            response.EnsureSuccessStatusCode();

            //herer we keep track of what was sent for the test oracle
            expectedComments.Add(comment);
        }

        //verify that each expected comment can be retrieved
        foreach (var expected in expectedComments)
        {
            var actualComments = await client.GetFromJsonAsync<List<Comment>>($"/comments?observationId={expected.ObservationId}");
            Assert.Contains(actualComments!, actual =>
                actual.ObservationId == expected.ObservationId &&
                actual.Author == expected.Author &&
                actual.Message == expected.Message &&
                actual.Timestamp == expected.Timestamp
            );
        }
    }
}