namespace Domain.Common;

using System.Buffers.Binary;

public record AggregateVersion(ulong Value)
{
    public static implicit operator ulong(AggregateVersion version) => version.Value;

    public static implicit operator AggregateVersion(ulong value) => new AggregateVersion(value);

    public static AggregateVersion FromBytes(byte[] bytes) =>
        BinaryPrimitives.ReadUInt64BigEndian(new ReadOnlySpan<byte>(bytes));

    public byte[] ToBytes()
    {
        var bytes = new byte[8];
        
        BinaryPrimitives.WriteUInt64BigEndian(new Span<byte>(bytes), Value);

        return bytes;
    }
}
