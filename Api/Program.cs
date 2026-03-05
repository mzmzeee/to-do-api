var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMemDb, MemDb>();

var app = builder.Build();

app.MapApiEndpoints();

app.Run();
