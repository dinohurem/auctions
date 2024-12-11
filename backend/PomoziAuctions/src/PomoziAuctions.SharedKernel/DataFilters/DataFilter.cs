using System.Collections.Concurrent;

namespace PomoziAuctions.SharedKernel.DataFilters;

public class DataFilter : IDataFilter
{
	private readonly ConcurrentDictionary<Type, bool> _dataFilters;

	public DataFilter()
	{
		_dataFilters = new ConcurrentDictionary<Type, bool>();
	}

	public IDisposable Disable<TFilter>() where TFilter : class
	{
		_dataFilters[typeof(TFilter)] = false;
		return new DisposableAction(() => Enable<TFilter>());
	}

	public IDisposable Enable<TFilter>() where TFilter : class
	{
		_dataFilters[typeof(TFilter)] = true;
		return new DisposableAction(() => Disable<TFilter>());
	}

	public bool IsEnabled<TFilter>() where TFilter : class
	{
		return _dataFilters.GetOrAdd(typeof(TFilter), true);
	}
}
