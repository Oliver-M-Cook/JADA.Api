
using JADA.Data.Context;
using JADA.Models.Database;
using JADA.Models.Request;
using JADA.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace JADA.Services.UserLogin;

public class UserLoginService : IUserLoginService
{
    private readonly UserDbContext _userDbContext;
    private readonly IPasswordHasher<UserLoginRequest> _passwordHasher;

    public UserLoginService(UserDbContext userDbContext, IPasswordHasher<UserLoginRequest> passwordHasher)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    public async Task<SuccessResponse> RegisterUserAsync(string email, string password)
    {
        if (_userDbContext.Users.Any(user => user.Email == email))
            return new(false, "User Already Exists");

        var userEntity = new UserEntity
        {
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(new(email, password), password)
        };

        _userDbContext.Users.Add(userEntity);
        await _userDbContext.SaveChangesAsync();

        return new(true, "User Created");
    }
}
