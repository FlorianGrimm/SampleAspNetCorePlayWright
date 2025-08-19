using System;
using System.Diagnostics;
using System.Numerics;

namespace Sample.BuisnessLogic.Model;

#if NETCOREAPP2_1_OR_GREATER
using System.Buffers.Binary;
#endif

#nullable enable

[DebuggerDisplay("{ToString(),nq}")]
public readonly struct MRowVersion : IComparable, IEquatable<MRowVersion>, IComparable<MRowVersion>
#if NET7_0_OR_GREATER
    , IComparisonOperators<MRowVersion, MRowVersion, bool>
#endif
{
    public static MRowVersion Zero => default;

    private readonly ulong value;

    public MRowVersion(ulong value) {
        this.value = value;
    }

#if !NETCOREAPP2_1_OR_GREATER
    public Rowversion(byte[] value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (value.Length != 8)
            throw new ArgumentException("The array does not have the correct length (8 bytes) to represent a SQL Server rowversion.", nameof(value));

        this.value =
            ((ulong)value[0] << 56)
            | ((ulong)value[1] << 48)
            | ((ulong)value[2] << 40)
            | ((ulong)value[3] << 32)
            | ((ulong)value[4] << 24)
            | ((ulong)value[5] << 16)
            | ((ulong)value[6] << 8)
            | value[7];
    }
#else
    public MRowVersion(ReadOnlySpan<byte> value) {
        this.value = BinaryPrimitives.ReadUInt64BigEndian(value);
    }
#endif

    public static implicit operator MRowVersion(ulong value) => new(value);

    public static implicit operator MRowVersion(long value) => new(unchecked((ulong)value));

    public static explicit operator MRowVersion(byte[] value) => new(value);

    public static explicit operator MRowVersion?(byte[]? value) => value is null ? null : new MRowVersion(value);

    public byte[] ToArray() {
        var array = new byte[8];

#if NETCOREAPP2_1_OR_GREATER
        WriteTo(array);
#else
        array[0] = (byte)(value >> 56);
        array[1] = (byte)(value >> 48);
        array[2] = (byte)(value >> 40);
        array[3] = (byte)(value >> 32);
        array[4] = (byte)(value >> 24);
        array[5] = (byte)(value >> 16);
        array[6] = (byte)(value >> 8);
        array[7] = (byte)value;
#endif

        return array;
    }

#if NETCOREAPP2_1_OR_GREATER
    public void WriteTo(Span<byte> destination) {
        BinaryPrimitives.WriteUInt64BigEndian(destination, value);
    }
#endif

    public override bool Equals(object? obj) => obj is MRowVersion other && Equals(other);

    public override int GetHashCode() => value.GetHashCode();

    public bool Equals(MRowVersion other) => other.value == value;

    int IComparable.CompareTo(object? obj) => obj == null ? 1 : CompareTo((MRowVersion)obj);

    public int CompareTo(MRowVersion other) => value.CompareTo(other.value);

    public static bool operator ==(MRowVersion left, MRowVersion right) => left.Equals(right);

    public static bool operator !=(MRowVersion left, MRowVersion right) => !left.Equals(right);

    public static bool operator >(MRowVersion left, MRowVersion right) => left.CompareTo(right) > 0;

    public static bool operator >=(MRowVersion left, MRowVersion right) => left.CompareTo(right) >= 0;

    public static bool operator <(MRowVersion left, MRowVersion right) => left.CompareTo(right) < 0;

    public static bool operator <=(MRowVersion left, MRowVersion right) => left.CompareTo(right) <= 0;

    public override string ToString() => value.ToString("x16");
}