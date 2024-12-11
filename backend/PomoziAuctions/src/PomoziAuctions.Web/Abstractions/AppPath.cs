
using PomoziAuctions.Core.Abstractions;

namespace PomoziAuctions.Abstractions.Web;

public class AppPath : IAppPath
{
	private readonly IWebHostEnvironment _hostEnvironment;

	public AppPath(IWebHostEnvironment hostEnvironment)
	{
		_hostEnvironment = hostEnvironment;
	}

	/// <inheritdoc />
	public string ResolveAppDataPath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException(nameof(path));
		}

		return Path.Combine(_hostEnvironment.ContentRootPath, "AppData", path);
	}
}
