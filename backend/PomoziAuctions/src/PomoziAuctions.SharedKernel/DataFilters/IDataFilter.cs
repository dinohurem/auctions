namespace PomoziAuctions.SharedKernel.DataFilters;

public interface IDataFilter
{
	IDisposable Enable<TFilter>() where TFilter : class;

	IDisposable Disable<TFilter>() where TFilter : class;

	bool IsEnabled<TFilter>() where TFilter : class;
}
