using System;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

// It automatically applies database migrations when your app starts
// It is a extension method for WebApplication, so you can call it in your Program.cs file like this: app.MigrateDb();
public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
                        .GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = "Data Source=GameStore.db";
        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            // Here Data seeding is added to the database, it will run every time the application starts, but it will only add data if the Genres table is empty.
            // Database seeding means inserting initial data into a database automatically when it is created or updated.
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Platformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" }
                    );

                    context.SaveChanges();
                    // If SQLite explorer is not seen then use ctrl + shift + p 
                    // and search for SQLite: Open Database and select the GameStore.db file in the project directory to view the database and its tables.
                }
            })

        );
    }
}
