using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

// Refactor the endpoints to a separate class to keep the Program.cs file clean and organized. 
// This also allows for better separation of concerns and makes it easier to maintain and test the endpoints in the future.
public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
    new (
        1,
        "Street Fighter II",
        "Fighting",
        19.99m,
        new DateOnly(1991, 7, 15)),
    new (
        2,
        "The Legend of Zelda: Ocarina of Time",
        "Action-Adventure",
        29.99m,
        new DateOnly(1998, 11, 21)),
    new (
        3,
        "Minecraft",
        "Sandbox",
        26.95m,
        new DateOnly(2011, 11, 18))

    ];

    public static void MapGameEndpoints(this WebApplication app)
    {

        // GET /games
        app.MapGet("/games", () => games);

        // GET /games/{id}
        app.MapGet("/games/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);


        // POST /games
        app.MapPost("/games", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games/{id}
        app.MapPut("/games/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // Delete /games/{id}
        // Unfortunately delete does not require a dto bcz it only requires the id and does not require any body content.
        app.MapDelete("/games/{id}", (int id) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games.RemoveAt(index);
            return Results.NoContent();
        });
    }
}
