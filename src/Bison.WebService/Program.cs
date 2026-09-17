using SimpleDB;
using Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var filePath = "../Bison.CLI/bison_observe_cli_db.csv";
var database = CSVDatabase<Observation>.getInstance(filePath);
var filePath2 = "../Bison.CLI/bison_comment_cli_db.csv";
var commentDb = CSVDatabase<Comment>.getInstance(filePath2);

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

app.Run();

public record ObservationRequest(string Author, string Message, long Timestamp, string? Location = null);
public record CommentRequest(int ObservationId, string Author, string Message, long Timestamp);