using System.Collections.Concurrent;
using Seventy.Common.Model;

namespace Seventy.WebService.Model;

public class InMemorySessionPool<TSessionData> where TSessionData : SessionData, new()
{
    public readonly ConcurrentDictionary<Guid, SessionDataWrapper<TSessionData>> DataCache = new();

    public void TerminateSessions()
    {
        foreach (var valuePair in DataCache)
            if (valuePair.Value is { IsInitialized: true, Data: IDisposable disposable })
                disposable.Dispose();

        DataCache.Clear();
    }
}
