namespace Shared.Contracts.Events.User;

    public record UserDeletedEvent
    {
        public string UserId { get; init; }
        public string CompanyId { get; init; }
        public DateTime DeletedAt { get; init; }
    }
