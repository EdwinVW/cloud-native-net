namespace Domain.Common;

/// <summary>
/// Represents an event that occured in the domain.
/// </summary>
public record Event
{
    /// <summary>
    /// The unique Id of the event. This Id is unique per event instance. This 
    /// is primarily used for logging and correlation.
    /// </summary>
    public Guid EventId { get; } = Guid.NewGuid();

    /// <summary>
    /// The type of the command. This value is primarily used for logging and 
    /// knowing which .NET type to use for deserialization from JSON.
    /// </summary>
    [JsonIgnore]
    public string Type => GetType().Name;
}
