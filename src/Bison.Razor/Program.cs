using Bison.Razor;
using Bison.Razor.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Use BISONDBPATH if provided, otherwise store the EF Core SQLite database in the system temp directory.
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? Path.Combine(Path.GetTempPath(), "bison-ef.db");

builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IObservationService, ObservationService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();


var app = builder.Build();

// Ensure the database exists, then seed it with the example data from DbInitializer.
using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();

    context.Database.EnsureCreated();

    DbInitializer.SeedDatabase(context);
}

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

//Needed for Razor pages integration tests to locate the program
public partial class Program {
}