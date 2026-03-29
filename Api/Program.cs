using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMemDb, MemDb>();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

app.MapControllers();

app.Run();
