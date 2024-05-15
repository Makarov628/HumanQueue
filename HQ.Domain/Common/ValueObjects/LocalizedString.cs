
using System.Text.Json;
using ErrorOr;
using HQ.Domain.Common.Models;

namespace HQ.Domain.Common.ValueObjects;


public record CultureString(
    string Culture,
    string Value
);

public class LocalizedString : ValueObject
{
    public List<CultureString> StringParts { get; private set; } = new();

    public static ErrorOr<LocalizedString> Create(List<CultureString> cultureStrings)
    {
        var unavailableCultures = cultureStrings
            .Where(part => !AvailableCultures.IsAvailable(part.Culture))
            .Select(part => part.Culture);
    
        if (unavailableCultures.Any())
            return Error.Validation(description: $"Данные культуры не доступны: {string.Join(", ", unavailableCultures)}");

        return new LocalizedString()
        {
            StringParts = cultureStrings
        };
    }

    public static LocalizedString CreateFromJson(string jsonString)
    {
        var deserializedStringParts = JsonSerializer.Deserialize<List<CultureString>>(jsonString) ?? new();

        return new LocalizedString()
        {
            StringParts = deserializedStringParts
        };
    }

    public CultureString? GetStringPartByCulture(string cultureName)
    {
        return StringParts.FirstOrDefault(part => part.Culture == cultureName);
    }
    
    public CultureString? GetStringPartByCulture(Culture culture)
    {
        return StringParts.FirstOrDefault(part => part.Culture == culture.Name);
    }

    public string? GetValueByCulture(string cultureName)
    {
        return StringParts.FirstOrDefault(part => part.Culture == cultureName)?.Value;
    } 

    public string? GetValueByCulture(Culture culture)
    {
        return StringParts.FirstOrDefault(part => part.Culture == culture.Name)?.Value;
    } 

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(StringParts);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return StringParts;
    }
}


