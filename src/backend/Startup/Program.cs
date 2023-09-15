using Accounts;
using Accounts.Data;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAccountsModule(builder.Configuration);

var app = builder.Build();

await app.ApplyMigrations<AccountsDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/hello", (string? name) =>
{
    name ??= "Anonymous user";
    return $"Hello, {name}!";
});

app.MapAccountsModule();

app.Run();
