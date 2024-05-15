

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ErrorOr;
using HQ.Domain.Common.Models;

namespace HQ.Domain.Common.ValueObjects;

public class Culture : ValueObject
{
    public string Name { get; private set; }
    public string LanguageName { get; private set; }

    public static ErrorOr<Culture> Create(string cultureName)
    {
        if (!AvailableCultures.IsAvailable(cultureName))
            return Error.Validation(description: $"Культура '{cultureName}' не доступна.");

        var cultureInfo = new CultureInfo(cultureName);
        return new Culture()
        {
            Name = cultureName,
            LanguageName = cultureInfo.NativeName
        };
    }

    public static Culture CreateWithCultureInfo(CultureInfo cultureInfo)
    {
        return new Culture()
        {
            Name = cultureInfo.Name,
            LanguageName = cultureInfo.NativeName
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }

    private Culture() {}
}

public static class AvailableCultures
{
    private readonly static List<Culture> cultures = new();

    public static void AppendCulture(string cultureName)
    {
        if (!CultureInfo.GetCultures(CultureTypes.AllCultures).Any(info => info.Name == cultureName))
            throw new NotSupportedException($"Культура '{cultureName}' не поддерживается.");

        var cultureInfo = new CultureInfo(cultureName);
        cultures.Add(Culture.CreateWithCultureInfo(cultureInfo));
    }

    public static bool IsAvailable(string cultureName)
    {
        return cultures.Any(culture => culture.Name == cultureName);
    }

    public static IReadOnlyList<Culture> GetCultures()
    {
        return cultures.AsReadOnly();
    }
}
