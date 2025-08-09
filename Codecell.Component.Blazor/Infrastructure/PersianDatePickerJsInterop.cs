using Microsoft.JSInterop;

namespace Codecell.Component.Blazor
{

    public class PersianDatePickerJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public PersianDatePickerJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Codecell.Component.Blazor/persianDatePicker.js").AsTask());
        }

        public async ValueTask<DomRect?> GetElementPositionAsync(string childSelector, string parentSelector)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<DomRect?>("getElementPosition", childSelector, parentSelector);
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
        public async Task ResetMask(string elemntId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("resetMask", elemntId);
        }
        public async Task<string> GetValue(string elemntId)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getValue", elemntId);
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

    public class DomRect
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
