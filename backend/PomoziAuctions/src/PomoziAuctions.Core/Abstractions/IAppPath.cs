namespace PomoziAuctions.Core.Abstractions;

public interface IAppPath
{
	/// <summary>
	/// Resolves app data path.
	/// </summary>
	/// <param name="path">Path relative to "App_Data".</param>
	/// <returns></returns>
	string ResolveAppDataPath(string path);
}
