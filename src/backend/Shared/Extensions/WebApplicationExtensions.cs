using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrations<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        //if (app.Environment.IsDevelopment())
        //{
            await using var scope = app.Services.CreateAsyncScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            await dbContext.Database.MigrateAsync();
        //}
    }
}
