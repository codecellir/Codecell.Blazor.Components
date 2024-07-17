using Microsoft.JSInterop;

namespace Codecell.Component.Blazor
{

    public class CodecellJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public CodecellJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Codecell.Component.Blazor/codecell.js").AsTask());
        }

        public async Task AddOutSideClickHandler(string elemntId, object dotNetObject)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("addOutSideClickHandler", elemntId, dotNetObject);
        }
        public async Task RemoveOutSideClickHandler(string elemntId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("removeOutSideClickHandler", elemntId);
        }
        public async Task AddDateMask(string elemntId, object dotNetObject)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("dateMask", elemntId, dotNetObject);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
