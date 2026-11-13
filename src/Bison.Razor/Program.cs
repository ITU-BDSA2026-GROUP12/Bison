using Bison.Razor;
using Bison.Razor.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BisonDBContextConnection") ?? throw new InvalidOperationException("Connection string 'BisonDBContextConnection' not found.");

// Add services to the container.
builder.Services.AddRazorPages();

// Register an in-memory cache for session storage.
builder.Services.AddDistributedMemoryCache();

// Register session services before building the application.
builder.Services.AddSession();

// Use BISONDBPATH if provided, otherwise store the EF Core SQLite database in the system temp directory.
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? Path.Combine(Path.GetTempPath(), "bison-ef.db");

builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<BisonDBContext>();

builder.Services.AddScoped<IObservationService, ObservationService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITaxonFilterService, TaxonFilterService>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
.AddCookie()
.AddGitHub(o => {
    o.ClientId = builder.Configuration["authentication:github:clientId"]
        ?? "";
    o.ClientSecret = builder.Configuration["authentication:github:clientSecret"]
        ?? "";
    o.CallbackPath = "/signin-github";
});

var app = builder.Build();

// Ensure the database exists, then seed it with the example data from DbInitializer.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();

    context.Database.EnsureCreated();

    // Open a connection to check whether the Posts table already contains the new optional PictureUrl column.
    var connection = context.Database.GetDbConnection();
    connection.Open();

    try {
        // Retrieve information about the existing columns.
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info('Posts');";

        using var reader = command.ExecuteReader();

        bool pictureColumnExists = false;

        // Check if PictureUrl already exists in the database.
        while (reader.Read()) {
            if (reader.GetString(1) == "PictureUrl") {
                pictureColumnExists = true;
                break;
            }
        }

        reader.Close();

        // Add PictureUrl only if it does not already exist.
        // NULL allows older observations to remain without pictures.
        if (!pictureColumnExists) {
            context.Database.ExecuteSqlRaw("ALTER TABLE Posts ADD COLUMN PictureUrl TEXT NULL;");
        }
    }
    finally {
        // Always close the connection, even if an error occurs.
        connection.Close();
    }

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

app.UseAuthentication();

app.UseAuthorization();

//app.UseSession();

app.MapRazorPages();

app.Run();

//Needed for Razor pages integration tests to locate the program
public partial class Program
{
}