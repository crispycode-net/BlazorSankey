using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Codeus.BlazorSankey
{
    public class SankeyJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public SankeyJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Codeus.BlazorSankey/sankey.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask<Dimensions> GetDimensions(ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<Dimensions>("getDimensions", element);
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

    public class Dimensions
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }
}