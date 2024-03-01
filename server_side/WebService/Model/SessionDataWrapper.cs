using Seventy.Common.Model;

namespace Seventy.WebService.Model;

public class SessionDataWrapper<TSessionData> where TSessionData : SessionData, new()
{
    private int _isInUse;

    public TSessionData Data { get; internal set; } = default!;

    public bool IsInitialized { get; private set; }

    internal SessionDataWrapper()
    {
        _isInUse = 0; // Not in use
    }

    internal void Init(AppUser user)
    {
        Program.InitSession(user, Data);

        IsInitialized = true;
    }

    public bool IsInUse
    {
        get => Interlocked.CompareExchange(ref _isInUse, 0, 0) == 1;
        set
        {
            if (value)
                Interlocked.Exchange(ref _isInUse, 1); // Set to in use

            else
                Interlocked.Exchange(ref _isInUse, 0); // Set to not in use

            IsInUseChanged?.Invoke(this, value);
        }
    }

    public bool TrySetInUse()
    {
        // Try to set _isInUse to 1 only if its current value is 0
        var wasSet = Interlocked.CompareExchange(ref _isInUse, 1, 0) == 0;

        if (wasSet)
            // The method successfully set _isInUse to 1
            IsInUseChanged?.Invoke(this, true);

        return wasSet;
    }

    public event EventHandler<bool>? IsInUseChanged;
}
