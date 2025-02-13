namespace Shared.Contracts.Events.Company;

public record class CompanyUpdatedEvent
{
    public string CompanyId { get; init; }
    public string Name { get; init; }
    public string OwnerId { get; init; }
    public DateTime UpdatedAt { get; init; }
}
