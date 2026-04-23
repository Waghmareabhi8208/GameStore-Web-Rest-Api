using FluentValidation;
using GameStore.Api.Data;
using GameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGameEndpoints();

app.MigrateDb();
app.Run();
