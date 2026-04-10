using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NeoUI.Blazor;
using NeoUI.Blazor.Extensions;
using NeoUI.Blazor.Primitives.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddNeoUIPrimitives();
builder.Services.AddNeoUIComponents();
builder.Services.AddScoped<MyWebTemplateV2.Client.Services.AuthService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
