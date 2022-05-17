namespace ContractManagement.Domain.ValueObjects;

public record ContractNumber
{
    public const int MaxLength = 6;

    // allowed pattern is 'CTR-', followed by the date (yyyyMMdd), followed by a sequence 
    // number of 4 digits (zero-filled and right aligned) An example: CTR-202204024-0021
    // The date in the number represents the date the contract was registered (this date  
    // can differ from the start- and end-date of the contract itself.
    private static readonly Regex _regex = 
        new(@"^CTR-\d{8}-\d{4}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

    public string Value { get; }

    private ContractNumber(string value)
    {
        Value = value;
    }

    public static ContractNumber Parse(string value)
    {
        if (TryParse(value, out var contractNumber))
        {
            return contractNumber;
        }

        throw new InvalidValueObjectException("Specified contract number is not valid. " + 
        "Allowed pattern is 'CTR-', followed by the date (yyyyMMdd), followed by " + 
        "a sequence number of 4 digits (zero-filled and right aligned)");
    }

    public static bool TryParse(
        string value,
        [MaybeNullWhen(false)] out ContractNumber contractNumber)
    {
        if (_regex.IsMatch(value))
        {
            contractNumber = new ContractNumber(value);
            return true;
        }

        contractNumber = null;
        return false;
    }
}