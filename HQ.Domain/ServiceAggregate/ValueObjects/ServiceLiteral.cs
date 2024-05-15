

using HQ.Domain.Common.Models;
using ErrorOr;

namespace HQ.Domain.ServiceAggregate.ValueObjects;


public sealed class ServiceLiteral : ValueObject
{
    public string Value { get; private set; }

    public static ErrorOr<ServiceLiteral> Create(string value)
    {
        if (!LiteralValidation.isValid(value))
            return Error.Validation(description: $"Неверное значение: '{value}'. Используйте символы: {LiteralValidation.AvailableChars}");
            
        var serviceLiteral = new ServiceLiteral()
        {
            Value = value
        };

        return serviceLiteral;
    }

    public static ServiceLiteral? Build(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        return new ServiceLiteral()
        {
            Value = value
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
       yield return Value;
    }
}


internal static class LiteralValidation 
{
    private static char[] AllowedCharacters = new char[] 
    { 
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 
        'H', 'I', 'K', 'L', 'M', 'N', 'O', 
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
        'W', 'X', 'Y', 'Z', '0', '1', '2',
        '3', '4', '5', '6', '7', '8', '9'
    };

    public static string AvailableChars => string.Join(", ", AllowedCharacters);

    public static bool isValid(string value)
    {
        foreach (char character in value)
        {
            if (!AllowedCharacters.Contains(character))
                return false;
        }

        return true;
    }
}
