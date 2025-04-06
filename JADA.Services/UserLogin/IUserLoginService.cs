using JADA.Models.Response;

namespace JADA.Services.UserLogin;

public interface IUserLoginService
{
    Task<SuccessResponse> RegisterUserAsync(string email, string password);
}
