using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Services;
using MudBlazor.Services;
using ApexCharts;

namespace MyWebTemplateV2.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        var systemConnectionString = configuration.GetConnectionString("SystemConnection");
        services.AddDbContext<SystemDbContext>(options =>
            options.UseMySql(systemConnectionString, ServerVersion.AutoDetect(systemConnectionString)));

        services.AddSingleton<AiChatService>();
        services.AddScoped<AuthService>();
        services.AddScoped<MyWebTemplateV2.Client.Services.AuthService>();
        services.AddScoped<FileUploadService>();
        
        // Use local address for server-side HTTP client if needed, or dynamic
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3002") });
        
        services.AddMudServices();
        services.AddApexCharts();
        services.AddSignalR();

        return services;
    }
}
