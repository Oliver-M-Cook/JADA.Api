using JADA.Data.Context;
using JADA.Models.Request;
using JADA.Services.UserLogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));
});

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await dbContext.Database.MigrateAsync();
}

builder.Services.AddScoped<IPasswordHasher<UserLoginRequest>, PasswordHasher<UserLoginRequest>>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

await builder.Build().RunAsync();
