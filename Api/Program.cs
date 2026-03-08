var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMemDb, MemDb>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
