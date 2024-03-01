using Seventy.Common.Model;
using Seventy.WebService.Model;

namespace Seventy.WebService.Utils.Services;

public class SessionService<TSessionData> where TSessionData : SessionData, new()
{
    private readonly InMemorySessionPool<TSessionData> _inMemorySessionPool;

    private readonly RefGuidService _dataReference;

    private Guid RefGuid => _dataReference.Value!.Value;

    public SessionService(InMemorySessionPool<TSessionData> inMemorySessionPool, RefGuidService dataReference)
    {
        _inMemorySessionPool = inMemorySessionPool;
        _dataReference = dataReference;
    }

    public TSessionData CreateData()
    {
        var data = new TSessionData();

        _dataReference.Value = Guid.NewGuid();

        var dataWrapper = new SessionDataWrapper<TSessionData> { Data = data };

        _inMemorySessionPool.DataCache.TryAdd(_dataReference.Value.Value, dataWrapper);

        return data;
    }

    public void ReleaseData()
    {
        if (_inMemorySessionPool.DataCache.TryGetValue(RefGuid, out var dw))
            dw.IsInUse = false;
    }

    public async Task RemoveDataFromCacheAsync()
    {
        if (!_inMemorySessionPool.DataCache.TryGetValue(RefGuid, out var dw))
            return;

        if (!await CaptureDataAsync())
            throw new Exception("Could not remove data from cache.");

        if (dw is { IsInitialized: true, Data: IDisposable disposable })
            disposable.Dispose();

        _inMemorySessionPool.DataCache.TryRemove(RefGuid, out _);

        dw.IsInUse = false;
    }

    public TSessionData RetrieveData() => RetrieveDataWrapper().Data;

    internal SessionDataWrapper<TSessionData> RetrieveDataWrapper()
    {
        if (_inMemorySessionPool.DataCache.TryGetValue(RefGuid, out var dw))
            return dw;

        throw new Exception("Cached data not found.");
    }

    public async Task<bool> CaptureDataAsync()
    {
        if (!_inMemorySessionPool.DataCache.TryGetValue(RefGuid, out var dw))
            return false;

        if (dw.TrySetInUse())
            return true;

        var tcs = new TaskCompletionSource<bool>();

        void IsInUseChangedHandler(object? sender, bool isInUse)
        {
            if (isInUse)
                return;

            dw.IsInUseChanged -= IsInUseChangedHandler;
            tcs.SetResult(true);
        }

        dw.IsInUseChanged += IsInUseChangedHandler;

        await tcs.Task;

        return await CaptureDataAsync();
    }
}
