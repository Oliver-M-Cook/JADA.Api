namespace JADA.Models.Request;

public record UserSignUpRequest(string Email, string Password, string ConfirmPassword);
