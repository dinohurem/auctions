using System.Text.RegularExpressions;

namespace PomoziAuctions.Core.Extensions;
public static class StringExtensions
{
	public static string RemoveWhiteSpace(this string source) =>
		source is null ? null : Regex.Replace(source, @"\s", string.Empty);

	public static TEnum ToEnum<TEnum>(this string source, bool ignoreCase = true)
		where TEnum : struct, IConvertible, IFormattable, IComparable
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"Type {typeof(TEnum)} has to be enum.");
		}

		return Enum.TryParse(source.RemoveWhiteSpace(), ignoreCase, out TEnum result) ? result : default;
	}
}
