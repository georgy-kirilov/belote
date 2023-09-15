using Accounts.Data;
using Accounts.Data.Models;
using Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace Accounts;

public static class ModuleRegistration
{
    public static IServiceCollection AddAccountsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetDefaultConnectionString();

        services.AddDbContext<AccountsDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        var passwordOptions = configuration.GetRequiredValue<PasswordOptions>("PasswordOptions");
        var requireConfirmedAccount = true;
        var requireUniqueEmail = true;

        services.AddIdentity<User, Role>(options =>
        {
            options.Password = passwordOptions;
            options.User.RequireUniqueEmail = requireUniqueEmail;
            options.SignIn.RequireConfirmedAccount = requireConfirmedAccount;
        })
        .AddEntityFrameworkStores<AccountsDbContext>();

        return services;
    }

    public static IEndpointRouteBuilder MapAccountsModule(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register", async (RegisterRequest request, UserManager<User> userManager) =>
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                Results.BadRequest(result.Errors);
            }

            return Results.Ok(new { UserId = user.Id });
        });

        app.MapGet("/accounts/{userId}", async (Guid userId, UserManager<User> userManager) =>
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Results.NotFound($"User with ID '{userId}' not found.");
            }

            return Results.Ok(new { Username = user.UserName, user.Email });
        });

        return app;
    }
}

public sealed record RegisterRequest(string Username, string Email, string Password);
