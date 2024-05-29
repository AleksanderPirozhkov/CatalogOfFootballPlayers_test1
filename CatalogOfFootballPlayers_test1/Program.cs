using CatalogOfFootballPlayers_test1.Data;
using CatalogOfFootballPlayers_test1.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CatalogOfFootballPlayersContextConnection")
    ?? throw new InvalidOperationException("Connection string 'CatalogOfFootballPlayersContextConnection' not found.");

builder.Services.AddDbContext<CatalogOfFootballPlayersContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options =>
{
    //options.MaximumReceiveMessageSize = 102400000;
    //options.EnableDetailedErrors = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapDefaultControllerRoute();
app.MapHub<FootballersHub>("/footballersHub");

app.Run();
