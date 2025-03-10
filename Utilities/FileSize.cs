using System.Diagnostics.CodeAnalysis;

namespace Garrison.Lib.Utilities;

public readonly partial struct FileSize(ulong value) : IFormattable
{
    private static readonly string[] s_units = ["B", "KB", "MB", "GB", "TB", "PB"];
    private static readonly uint s_defaultPrecision = 2;

    public static explicit operator FileSize(uint value) => (FileSize)(ulong)value;
    public static explicit operator FileSize(int value) => (FileSize)(long)value;
    public static explicit operator FileSize(double value) => (FileSize)(long)value;
    public static explicit operator FileSize(ulong value) => new(value);
    public static explicit operator FileSize(long value)
    {
        if (value < 0) throw new NotSupportedException("Cannot have a negative FileSize");
        return new((ulong)value);
    }

    public static bool operator <(FileSize a, FileSize b) => a._value < b._value;
    public static bool operator >(FileSize a, FileSize b) => a._value > b._value;
    public static bool operator <=(FileSize a, FileSize b) => a._value <= b._value;
    public static bool operator >=(FileSize a, FileSize b) => a._value >= b._value;
    public static bool operator ==(FileSize a, FileSize b) => a._value == b._value;
    public static bool operator !=(FileSize a, FileSize b) => a._value != b._value;

    public static bool operator <(FileSize size, long operand) => operand >= 0 && size._value < (ulong)operand;
    public static bool operator >(FileSize size, long operand) => operand < 0 || size._value > (ulong)operand;
    public static bool operator <(long operand, FileSize size) => operand < 0 || (ulong)operand < size._value;
    public static bool operator >(long operand, FileSize size) => operand >= 0 && (ulong)operand > size._value;

    public static bool operator <=(FileSize size, long operand) => operand >= 0 && size._value <= (ulong)operand;
    public static bool operator >=(FileSize size, long operand) => operand < 0 || size._value >= (ulong)operand;
    public static bool operator <=(long operand, FileSize size) => operand < 0 || (ulong)operand <= size._value;
    public static bool operator >=(long operand, FileSize size) => operand >= 0 && (ulong)operand >= size._value;

    public static bool operator ==(FileSize size, long operand) => operand >= 0 && size._value == (ulong)operand;
    public static bool operator !=(FileSize size, long operand) => operand < 0 || size._value != (ulong)operand;
    public static bool operator ==(long operand, FileSize size) => operand >= 0 && size._value == (ulong)operand;
    public static bool operator !=(long operand, FileSize size) => operand < 0 || size._value != (ulong)operand;

    public static FileSize operator *(FileSize size, double operand) => size * operand;
    public static FileSize operator *(double operand, FileSize size)
    {
        if (operand < 0) throw new NotSupportedException("Cannot multiply a filesize by a negative number");
        return new((ulong)(size._value * operand));
    }

    public static FileSize operator /(FileSize size, double operand)
    {
        if (operand < 0) throw new NotSupportedException("Cannot divide a filesize by a negative number");
        return new((ulong)(size._value / operand));
    }

    private readonly ulong _value = value;

    public override readonly string ToString() => ToString(null, null);

    public readonly string ToString(string format) => ToString(format, null);

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        var x = this / 12345;
        if (string.IsNullOrEmpty(format))
            return ToString(s_defaultPrecision);
        else if (uint.TryParse(format, out var precision))
            return ToString(precision);
        else
            return _value.ToString(format, formatProvider);
    }

    public readonly string ToString(uint precision)
    {
        var pow = Math.Min(s_units.Length - 1, Math.Floor((_value > 0 ? Math.Log(_value) : 0) / Math.Log(1024)));
        var unit = s_units[(int)pow];
        var value = _value / Math.Pow(1024, pow);

        return pow switch
        {
            0 => $"{value:F0} {unit}",
            _ => string.Format($"{{0:F{precision}}} {{1}}", value, unit)
        };
    }
}

partial struct FileSize : IEquatable<FileSize>, IComparable<FileSize>
{

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is FileSize other && Equals(other);
    public readonly bool Equals(FileSize other) => this == other;
    public override int GetHashCode() => _value.GetHashCode();
    public int CompareTo(FileSize other) => _value.CompareTo(other._value);
}