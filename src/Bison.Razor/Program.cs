using Bison.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Uses BISONPATH, if no path is found uses bison.db on you temp
builder.Services.AddSingleton<DBFacade>(_ =>
{
    var dbPath =
        Environment.GetEnvironmentVariable("BISONDBPATH")
        ?? Path.Combine(Path.GetTempPath(), "bison.db");

    return new DBFacade(dbPath);
});
builder.Services.AddSingleton<IObservationService, ObservationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
