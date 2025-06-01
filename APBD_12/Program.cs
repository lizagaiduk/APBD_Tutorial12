using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<TripDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IClientService, ClientService>();


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();