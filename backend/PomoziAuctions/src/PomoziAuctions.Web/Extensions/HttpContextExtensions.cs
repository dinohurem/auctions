using System.Globalization;

namespace PomoziAuctions.Web.Extensions;

public static class HttpContextExtensions
{
	private const string DefaultCulture = "en-US";

	public static string GetCulture(this HttpContext context)
	{
		var request = context?.Request;
		if (request == null)
		{
			return DefaultCulture;
		}

		var acceptHeader = request.Headers["Accept-Language"].FirstOrDefault();
		var result = !string.IsNullOrEmpty(acceptHeader)
			? acceptHeader.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]
				: null;

		result = string.IsNullOrEmpty(result) ? DefaultCulture : result;

		try
		{
			return CultureInfo.GetCultureInfo(result).Name;
		}
		catch (CultureNotFoundException)
		{
			return DefaultCulture;
		}
	}
}
