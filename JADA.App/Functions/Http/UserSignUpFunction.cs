using JADA.Models.Request;
using JADA.Models.Response;
using JADA.Services.UserLogin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JADA.App.Functions.Http;

public class UserSignUpFunction
{
    private readonly IUserLoginService _userLoginService;

    public UserSignUpFunction(IUserLoginService userLoginService)
    {
        _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
    }

    [Function("UserSignUp")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/signup")] HttpRequest req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var userSignUpRequest = JsonConvert.DeserializeObject<UserSignUpRequest>(body);

        if (userSignUpRequest == null)
            return new BadRequestObjectResult(new SuccessResponse(false, "Bad Payload"));

        if (string.IsNullOrEmpty(userSignUpRequest.Email) || string.IsNullOrEmpty(userSignUpRequest.Password) || string.IsNullOrEmpty(userSignUpRequest.ConfirmPassword))
            return new BadRequestObjectResult(new SuccessResponse(false, "Email and Password are required"));

        if (userSignUpRequest.Password != userSignUpRequest.ConfirmPassword)
            return new BadRequestObjectResult(new SuccessResponse(false, "Passwords do not match"));

        var result = await _userLoginService.RegisterUserAsync(userSignUpRequest.Email, userSignUpRequest.Password);

        return result.IsSuccess
            ? new OkObjectResult(result)
            : new BadRequestObjectResult(result);
    }
}
