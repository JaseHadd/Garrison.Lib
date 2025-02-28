namespace Garrison.Lib.DataAnnotations;

[AttributeUsage(AttributeTargets.Property)]
public abstract class CaseSensitivityAttribute(bool caseSensitive) : Attribute
{
    public bool CaseSensitive { get; } = caseSensitive;
}

public class CaseSensitiveAttribute() : CaseSensitivityAttribute(true);

public class CaseInsensitiveAttribute() : CaseSensitivityAttribute(false);