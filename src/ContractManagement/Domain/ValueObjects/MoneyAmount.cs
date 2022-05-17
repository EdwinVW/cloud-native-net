namespace ContractManagement.Domain.ValueObjects;

public record MoneyAmount
{
    public decimal Value { get; }

    private MoneyAmount(decimal value)
    {
        Value = value;
    }

    public static MoneyAmount Parse(decimal value)
    {
        if (TryParse(value, out var moneyAmount))
        {
            return moneyAmount;
        }

        throw new InvalidValueObjectException("Specified amount is not valid.");
    }

    public static bool TryParse(
        decimal value,
        [MaybeNullWhen(false)] out MoneyAmount moneyAmount)
    {
        // MoneyAmount cannot be negative
        if (value > 0)
        {
            moneyAmount = new MoneyAmount(value);
            return true;
        }

        moneyAmount = null;
        return false;
    }
}