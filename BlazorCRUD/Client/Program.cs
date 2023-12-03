global using BlazorCRUD.Shared;
global using System.Net.Http.Json;
global using BlazorCRUD.Client.Services.ProductService;
global using BlazorCRUD.Client.Services.CategoryService;
using BlazorCRUD.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using BlazorCRUD.Client.Services.CartService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => { return new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };  }) ;
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

await builder.Build().RunAsync();
