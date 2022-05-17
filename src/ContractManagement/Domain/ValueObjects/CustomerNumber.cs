namespace ContractManagement.Domain.ValueObjects;

public record CustomerNumber
{
    public const int MaxLength = 6;

    // Allowed pattern is a 'C' followed by 5 digits (99999).
    private static readonly Regex _regex = new(@"^C\d{5}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

    public string Value { get; }

    private CustomerNumber(string value)
    {
        Value = value;
    }

    public static CustomerNumber Parse(string value)
    {
        if (TryParse(value, out var customerNumber))
        {
            return customerNumber;
        }

        throw new InvalidValueObjectException("Specified customer number is not valid. " +
            "Allowed pattern is a 'C' followed by 5 digits (99999).");
    }

    public static bool TryParse(
        string value,
        [MaybeNullWhen(false)] out CustomerNumber customerNumber)
    {
        if (_regex.IsMatch(value))
        {
            customerNumber = new CustomerNumber(value);
            return true;
        }

        customerNumber = null;
        return false;
    }
}