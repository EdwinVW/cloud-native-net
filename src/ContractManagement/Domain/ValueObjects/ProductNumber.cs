namespace ContractManagement.Domain.ValueObjects;

public record ProductNumber
{
    public const int MaxLength = 6;

    // Allowed pattern is 'FAC-', followed by 5 digits product code.
    // The abbreviation 'FAC' comes from 'Facility' (the term used for products in the Product Management domain).
    private static readonly Regex _regex = new(@"^FAC-\d{5}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

    public string Value { get; }

    private ProductNumber(string value)
    {
        Value = value;
    }

    public static ProductNumber Parse(string value)
    {
        if (TryParse(value, out var productNumber))
        {
            return productNumber;
        }

        throw new InvalidValueObjectException("Specified product number is not valid. " +
            "Allowed pattern is 'FAC-', followed by 5 digits product code.");
    }

    public static bool TryParse(
        string value,
        [MaybeNullWhen(false)] out ProductNumber productNumber)
    {
        if (_regex.IsMatch(value))
        {
            productNumber = new ProductNumber(value);
            return true;
        }

        productNumber = null;
        return false;
    }
}