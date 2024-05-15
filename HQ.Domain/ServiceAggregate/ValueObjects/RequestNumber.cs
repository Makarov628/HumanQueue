

using HQ.Domain.Common.Models;

namespace HQ.Domain.ServiceAggregate.ValueObjects;

public sealed class RequestNumber : ValueObject
{
    public ServiceLiteral ServiceLiteral { get; private set; }
    public int Number { get; private set; }

    public static RequestNumber Create(ServiceLiteral literal, int number)
    {
        return new()
        {
            ServiceLiteral = literal,
            Number = number
        };
    }

    public static RequestNumber BuildFromString(string value)
    {
        var numberParts = value.Split('-');
        var literal = ServiceLiteral.Create(numberParts[0]).Value;
        var number = Convert.ToInt32(numberParts[1]);
        return new RequestNumber()
        {
            ServiceLiteral = literal,
            Number = number
        };
    }

    public string ConvertToPersistString()
    {
        return $"{ServiceLiteral.Value}-{Number}";
    }

    public override string ToString()
    {
        return $"{ServiceLiteral.Value}-{Number:00}";
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ServiceLiteral.Value;
        yield return Number;
    }
}