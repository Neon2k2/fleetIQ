namespace Shared.Contracts.Events.Company;

    public record CompanyCreatedEvent
    {
        public string CompanyId { get; init; }
        public string Name { get; init; }
        public string OwnerId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
