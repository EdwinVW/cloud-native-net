namespace ContractManagement.Domain.Aggregates.Contract.DomainEvents;

/// <summary>
/// This event version is obsolete, but could still be part of the event-stream in the event-store. 
/// So events are never removed or made obsolete in the code.
/// </summary>
public record ContractRegistered(
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate) : Event;