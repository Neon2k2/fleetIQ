namespace AuthenticationService.Data.Dtos;

public record class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
