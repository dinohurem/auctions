using System.ComponentModel.DataAnnotations;

namespace PomoziAuctions.Core.Extensions;

public static class EnumExtensions
{
	public static (string Group, string Name, string Description, bool? AutoGenerateField) GetDisplayData<TEnum>(this TEnum value)
		where TEnum : struct, IComparable, IConvertible, IFormattable
	{
		if (!typeof(TEnum).IsEnum)
		{
			throw new ArgumentException($"Type {typeof(TEnum)} has to be enum.");
		}

		var displayAttribute = value.GetType()
			.GetField(value.ToString())
			.GetCustomAttributes(typeof(DisplayAttribute), false)
			.OfType<DisplayAttribute>()
			.FirstOrDefault();

		return (displayAttribute?.GetGroupName(), displayAttribute?.GetName(), displayAttribute?.GetDescription(), displayAttribute?.GetAutoGenerateField());
	}
}
