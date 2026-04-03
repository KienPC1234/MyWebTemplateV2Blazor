using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<MyWebTemplateV2.Client.Components.UI.ThemeService>();

await builder.Build().RunAsync();
