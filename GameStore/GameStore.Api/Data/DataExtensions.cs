using System;
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
}
