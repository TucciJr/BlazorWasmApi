using Blazored.LocalStorage;
using BlazorWasmApi.Web.Client.Pages;
using BlazorWasmApi.Web.Components;
using BlazorWasmApi.Web.Services;
using BlazorWasmApi.Web.Services.Contracts;

namespace BlazorWasmApi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7084/") });

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IManageProductsLocalStorageService, ManageProductsLocalStorageService>();
            builder.Services.AddScoped<IManageCartItemsLocalStorageService, ManageCartItemsLocalStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
