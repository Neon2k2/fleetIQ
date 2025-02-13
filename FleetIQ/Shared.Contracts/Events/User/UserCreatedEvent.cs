namespace Shared.Contracts.Events.User;

public record UserCreatedEvent
{
    public string UserId { get; init; }
    public string Email { get; init; }
    public string CompanyId { get; init; }
    public string Role { get; init; }
    public DateTime CreatedAt { get; init; }
}
