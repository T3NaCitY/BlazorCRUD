global using BlazorCRUD.Shared;
global using System.Net.Http.Json;
global using BlazorCRUD.Client.Services.ProductService;
global using BlazorCRUD.Client.Services.CategoryService;
using BlazorCRUD.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => { return new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };  }) ;
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

await builder.Build().RunAsync();
