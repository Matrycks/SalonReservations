using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SalonReservations;
using SalonReservations.DAL;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Repos;
using SalonReservations.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Add in-memory DbContext
builder.Services.AddDbContext<SalonDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

// Add services to the container.
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<RadzenMenu>();

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepo<>));
builder.Services.AddScoped<SalonServices>();
builder.Services.AddScoped<StylistService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AvailabilityService>();

var app = builder.Build();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SalonDbContext>();
    var availService = scope.ServiceProvider.GetRequiredService<AvailabilityService>();
    await SeedDatabase.Init(db, availService);
}

await builder.Build().RunAsync();
