using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWasmApi.Web.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7084/") });

        await builder.Build().RunAsync();
    }
}
