using Pms.Api.Models;
using Pms.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers keep the HTTP routes separate from the data storage code.
builder.Services.AddControllers();
builder.Services.AddSingleton<InMemoryProjectStore>();

var app = builder.Build();

app.MapControllers();

app.Run();
