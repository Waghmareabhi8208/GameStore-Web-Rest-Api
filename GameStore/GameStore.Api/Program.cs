using GameStore.Api.Dtos;
using GameStore.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGameEndpoints();
app.Run();
