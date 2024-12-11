using System.Globalization;

namespace PomoziAuctions.SharedKernel;

public class Language
{
	private const string EnUsLcidName = "en-US";

	/// <summary>
	///     Initializes a new instance of the <see cref="Language"/> class.
	/// </summary>
	/// <param name="cultureName">
	///     Culture name (not case-sensitive). Format languagecode2-countrycode2 or languagecode2.
	/// </param>
	public Language(string cultureName)
	{
		var culture = GetCultureInfoOrDefault(cultureName);
		ThreeLetterISORegionName = new RegionInfo(culture.LCID)?.ThreeLetterISORegionName;
		CultureName = culture.Name;
		EnglishName = culture.EnglishName;
		NativeName = culture.NativeName;
	}

	/// <summary>
	///     Gets the language culture name in the format languagecode2-countrycode2.
	/// </summary>
	public string CultureName { get; set; }

	/// <summary>
	///     Gets the full language name in English.
	/// </summary>
	public string EnglishName { get; set; }

	/// <summary>
	///     Gets the full language native name.
	/// </summary>
	public string NativeName { get; set; }

	/// <summary>
	///		Gets the three-letter code defined in ISO 3166 for the country/region.
	/// </summary>
	public string ThreeLetterISORegionName { get; set; }


	public static implicit operator string(Language language) => language?.CultureName;

	public static implicit operator Language(string cultureName) => new(cultureName);

	/// <summary>
	///     Returns <see cref="CultureName"/>.
	/// </summary>
	/// <returns>Culture name.</returns>
	public override string ToString() => CultureName;

	private static CultureInfo GetCultureInfoOrDefault(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return CultureInfo.GetCultureInfo(EnUsLcidName);
		}

		try
		{
			return CultureInfo.GetCultureInfo(name);
		}
		catch (CultureNotFoundException)
		{
			return CultureInfo.GetCultureInfo(EnUsLcidName);
		}
	}
}
