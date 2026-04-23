using System;
using GameStore.Api.Dtos;
using FluentValidation;
using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.EndPoints;

// Refactor the endpoints to a separate class to keep the Program.cs file clean and organized. 
// This also allows for better separation of concerns and makes it easier to maintain and test the endpoints in the future.
public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameSummaryDto> games = [
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
        // Create a group for all game-related endpoints to keep them organized and easily identifiable in the API documentation.
        var group = app.MapGroup("/games"); // 

        // GET /games
        group.MapGet("/",async (GameStoreContext dbContext) => 
            await dbContext.Games
                            .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleseDate
                            ))
                            .AsNoTracking()
                            .ToListAsync());

        // GET /games/{id}
        group.MapGet("/{id}", async (int id,GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound():Results.Ok(
                new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleseDate
            ));
        })
        .WithName(GetGameEndpointName);


        // POST /games
        group.MapPost("/", async (CreateGameDto newGame,GameStoreContext dbContext ,IValidator<CreateGameDto> validator) =>
        {
            var result = await validator.ValidateAsync(newGame);

            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors);
            }

           Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", async (
                int id,
                UpdateGameDto updateGame,
                GameStoreContext dbContext,
                IValidator<UpdateGameDto> validator) =>
                {
                    var validationResult = await validator.ValidateAsync(updateGame);

                    if (!validationResult.IsValid)
                    {
                        return Results.BadRequest(validationResult.Errors);
                    }

                    var existingGame = await dbContext.Games.FindAsync(id);

                    if (existingGame is null)
                    {
                        return Results.NotFound();
                    }

                    // Optional but recommended
                    var genreExists = await dbContext.Genres
                        .AnyAsync(g => g.Id == updateGame.GenreId);

                    if (!genreExists)
                    {
                        return Results.BadRequest("Invalid GenreId");
                    }

                    existingGame.Name = updateGame.Name;
                    existingGame.GenreId = updateGame.GenreId;
                    existingGame.Price = updateGame.Price;
                    existingGame.ReleseDate = updateGame.ReleaseDate;

                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                });

                
        // Delete /games/{id}
        // Unfortunately delete does not require a dto bcz it only requires the id and does not require any body content.
        group.MapDelete("/{id}", (int id) =>
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
