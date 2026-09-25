using SimpleDB;
using Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var filePath = "../Bison.CLI/bison_observe_cli_db.csv";
IDatabaseRepository<Observation> database = CSVDatabase<Observation>.getInstance(filePath);

var filePath2 = "../Bison.CLI/bison_comment_cli_db.csv";
IDatabaseRepository<Comment> commentDb = CSVDatabase<Comment>.getInstance(filePath2);

var filePath3 = "../Bison.CLI/bison_proposal_cli_db.csv";
IDatabaseRepository<Proposal> proposalDb = CSVDatabase<Proposal>.getInstance(filePath3);

//GET all observations from the database
app.MapGet("/observations", () => database.Read());

//POST new observation to the database
app.MapPost("/observation", (ObservationRequest request) =>
{
    //generate the next observation id
    int id = database.Read().Count() + 1;

    var observation = new Observation(id, request.Author, request.Message, request.Timestamp, request.Location);
    database.Store(observation);
});

//POST a comment belonging to an excisting observation
app.MapPost("/comment", (CommentRequest request) =>
{
    //check if observation that is being commented on exists.
    if (database.Read().Any(o => o.ObservationId == request.ObservationId))
    {
        var comment = new Comment(request.ObservationId, request.Author, request.Message, request.Timestamp);
        commentDb.Store(comment);
    }
});

//GET all comments belonging to a specific observation
app.MapGet("/comments", (int ObservationId) => { return commentDb.Read().Where(comment => comment.ObservationId == ObservationId); });

//POST a proposal belonging to an excisting observation
app.MapPost("/proposal", (ProposalRequest request) =>
{
    //check if observation that is being proposed to exists.
    if (database.Read().Any(o => o.ObservationId == request.ObservationId))
    {
        var proposal = new Proposal(request.ObservationId, request.Author, request.TaxonId, request.Timestamp);
        proposalDb.Store(proposal);
    }
});

//GET all proposals belonging to a specific observation
app.MapGet("/proposals", (int ObservationId) => { return proposalDb.Read().Where(proposal => proposal.ObservationId == ObservationId); });

app.Run();