namespace Shared.Contracts.Events.User;

public record UserUpdatedEvent
{
    public string UserId { get; init; }
    public string Email { get; init; }
    public string CompanyId { get; init; }
    public string Role { get; init; }
    public DateTime UpdatedAt { get; init; }
}
