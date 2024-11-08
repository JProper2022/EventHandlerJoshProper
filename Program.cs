using Microsoft.EntityFrameworkCore;
using EventHandlerJoshProper.Entities;
using EventHandlerJoshProper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MSWebDB");
builder.Services.AddDbContext<EventManagerDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IEventManagerService, EventManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
