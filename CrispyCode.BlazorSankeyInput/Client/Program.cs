using CrispyCode.BlazorSankeyInput;
using CrispyCode.BlazorSankeyInput.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CrispyCode.BlazorSankeyInput
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //builder.RootComponents.Add<App>("#app");
            //builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // register the Greeting component as a custom element
            builder.RootComponents.RegisterCustomElement<InputContainer>("input-container");

            await builder.Build().RunAsync();
        }
    }
}