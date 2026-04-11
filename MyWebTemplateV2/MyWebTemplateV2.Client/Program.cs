using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ApexCharts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<MyWebTemplateV2.Client.Services.AuthService>();
builder.Services.AddMudServices();
builder.Services.AddApexCharts();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
