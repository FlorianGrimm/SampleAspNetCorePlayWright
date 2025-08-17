using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public static class OptionsMonitorWrapper {
    public static OptionsMonitorWrapper<TOptions> Create<TOptions>(TOptions value)
        where TOptions : class {
        return new OptionsMonitorWrapper<TOptions>(value);
    }
}

/// <summary>
/// Wraps the options instance.
/// </summary>
/// <typeparam name="TOptions">The options type.</typeparam>
public class OptionsMonitorWrapper<TOptions> : IOptionsMonitor<TOptions>
    where TOptions : class {
    private readonly TOptions _Value;

    /// <summary>
    /// Initializes the wrapper with the options instance to return.
    /// </summary>
    /// <param name="options">The options instance to return.</param>
    public OptionsMonitorWrapper(TOptions options) {
        this._Value = options;
    }

    /// <summary>
    /// Gets the options instance.
    /// </summary>
    public TOptions Value => this._Value;

    public TOptions CurrentValue => this.Value;

    public TOptions Get(string? name) {
        return this.Value;
    }

    public IDisposable? OnChange(Action<TOptions, string?> listener) {
        return null;
    }
}
