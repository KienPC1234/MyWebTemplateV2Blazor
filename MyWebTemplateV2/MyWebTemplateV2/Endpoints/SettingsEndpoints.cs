using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Client;

namespace MyWebTemplateV2.Endpoints;

public static class SettingsEndpoints
{
    public static void MapSettingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{Constants.Api.Admin}/settings/{{key}}", async (SystemDbContext db, string key) => {
            var setting = await db.SiteSettings.FindAsync(key);
            if (setting == null) return Results.NotFound();
            return Results.Ok(setting);
        });

        app.MapPost($"{Constants.Api.Admin}/settings", async (HttpContext context, SystemDbContext db, SiteSetting setting) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();

            var existing = await db.SiteSettings.FindAsync(setting.Key);
            if (existing == null) {
                db.SiteSettings.Add(setting);
            } else {
                existing.Value = setting.Value;
            }
            await db.SaveChangesAsync();
            return Results.Ok(setting);
        });
    }
}
