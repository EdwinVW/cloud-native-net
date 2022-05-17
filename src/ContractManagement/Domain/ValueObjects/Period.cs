namespace ContractManagement.Domain.ValueObjects;

public record Duration
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private Duration(DateTime starteDate, DateTime endDate)
    {
        StartDate = starteDate;
        EndDate = endDate;
    }

    public static Duration Parse(DateTime startDate, DateTime endDate)
    {
        if (TryParse(startDate, endDate, out var duration))
        {
            return duration;
        }

        throw new InvalidValueObjectException("Specified start-time and/or end-time is not valid.");
    }

    public static bool TryParse(
        DateTime startDate,
        DateTime endDate,
        [MaybeNullWhen(false)] out Duration duration)
    {
        duration = new Duration(startDate, endDate);
        return true;
    }

    public override string ToString()
    {
        return $"From {StartDate.ToString("dd-MM-yyyy")} to {EndDate.ToString("dd-MM-yyyy")}";
    }
}