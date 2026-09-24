using System.Net.Http.Json;
using Model;
using Taxonomy;


//test generates 10 randomized proposals, sends them to the web service, and then checks that they were stored correctly and can be retrieved again.
public class ProposalsTest
{
    [Fact]
    public async Task FuzzProposals()
    {
        //set up client for communication
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5256");

        //set up taxonomy store and random generator
        var taxonomyStore = new TaxonomyStore();
        var random = new Random();

        //valid vernacular names used to retrieve taxa from the taxonomy store
        var taxonNames = new[]
        {
            "Fiskehejre",
              "Skarv"
            };

        //keep track of the proposals
        var expectedProposals = new List<ProposalRequest>();

        //get existing observations so proposals use valid ObservationIds
        var observations = await client.GetFromJsonAsync<List<Observation>>("/observations");

        //Generate and post 10 random proposals
        for (int i = 0; i < 10; i++)
        {
            //select random existing observation
            var randomObservation = observations![random.Next(observations.Count)];

            //select a random valid taxon
            var randomTaxonName = taxonNames[random.Next(taxonNames.Length)];

            var randomTaxon = taxonomyStore.GetByVernacularName(randomTaxonName);
            Assert.NotNull(randomTaxon);

            //create a proposal with randomized data
            var proposal = new ProposalRequest(
                ObservationId: randomObservation.ObservationId,
                Author: $"User{random.Next(1, 1000)}",
                TaxonId: randomTaxon!.TaxonId,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                );

            //send proposal
            var response = await client.PostAsJsonAsync("/proposal", proposal);
            response.EnsureSuccessStatusCode();

            //store
            expectedProposals.Add(proposal);
        }
        //retrieve the proposals and verify that each generated proposal was stored
        foreach (var expected in expectedProposals)
        {
            var actualProposal = await client.GetFromJsonAsync<List<Proposal>>($"/proposals?observationId={expected.ObservationId}");
            Assert.Contains(actualProposal!, actual =>
                actual.ObservationId == expected.ObservationId &&
                actual.Author == expected.Author &&
                actual.TaxonId == expected.TaxonId &&
                actual.Timestamp == expected.Timestamp
            );
        }
    }
}