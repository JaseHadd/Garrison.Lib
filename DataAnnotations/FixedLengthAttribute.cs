namespace Garrison.Lib.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public class FixedLengthAttribute(int length) : Attribute
{
    public int Length { get; } = length;
}