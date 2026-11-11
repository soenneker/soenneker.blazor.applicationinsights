using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Soenneker.Blazor.ApplicationInsights.Tests;

internal sealed class RecordingJsModule : IJSInProcessObjectReference
{
    public List<object?[]?> Calls { get; } = [];
    public bool ThrowOnInvoke { get; set; }

    public TValue Invoke<TValue>(string identifier, params object?[]? args)
    {
        if (ThrowOnInvoke)
            throw new JSException("SDK unavailable");
        if (identifier != "trackError")
            throw new InvalidOperationException(identifier);
        Calls.Add(args);
        return default!;
    }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => new(Invoke<TValue>(identifier, args));
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
        new(Invoke<TValue>(identifier, args));
    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
