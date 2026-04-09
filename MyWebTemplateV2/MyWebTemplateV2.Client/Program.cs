using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NeoUI.Blazor;
using NeoUI.Blazor.Extensions;
using NeoUI.Blazor.Primitives.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddNeoUIPrimitives();
builder.Services.AddNeoUIComponents();

await builder.Build().RunAsync();
