
public class ObservationTest
{
    [Fact]
    public async Task FuzzObservations()
    {
        //set up client for communication
        var client = new HttpClient();
        //our server that we'll communicate with
        client.BaseAddress = new Uri("http://localhost:5256");

        var random = new Random();
        var expectedObservations = new List<ObservationRequest>();

        //Make 10 observations
        for (int i = 0; i < 10; i++)
        {
            //generate random data for the observation
            var observation = new ObservationRequest(
                Author: $"User{random.Next(1, 1000)}",
                Message: $"Random message{random.Next(1, 1000)}",
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Location: $"Location{random.Next(1, 1000)}");


            //send observation
            var response = await client.PostAsJsonAsync("/observation", observation);

            //Ensure success. EnsureSucessStatusCode() is a part of .NET library
            response.EnsureSuccessStatusCode();

            //remember what we've sent
            expectedObservations.Add(observation);

        }

        //get observations
        var actualObservation = await client.GetFromJsonAsync<List<Observation>>("/observations");

        //Sammenlign forventet med faktisk resultat
        Console.WriteLine($"Sent:     {expectedObservations.Count}");
        Console.WriteLine($"Received: {actualObservation?.Count}");

        foreach (var expected in expectedObservations)
        {
            Assert.Contains(actualObservation!, actual =>
            actual.Author == expected.Author &&
            actual.Message == expected.Message &&
            actual.Timestamp == expected.Timestamp &&
            actual.Location == expected.Location);
        }
    }
}
