using BlazorWasmTest;
using CrispyCode.BlazorSankey;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWasmTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Comment out the following lines to publish custom elements
            //builder.RootComponents.Add<App>("#app");
            //builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // register the Greeting component as a custom element
            builder.RootComponents.RegisterCustomElement<SankeyDiagram>("sankey-diagram");

            await builder.Build().RunAsync();
        }
    }
}