using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Client;

namespace MyWebTemplateV2.Endpoints;

public static class PublicationEndpoints
{
    public static void MapPublicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(Constants.Api.Publications, async (AppDbContext db) => 
            await db.Publications.OrderByDescending(p => p.CreatedAt).ToListAsync());

        app.MapPost(Constants.Api.Publications, async (HttpContext context, AppDbContext db, Publication pub) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();
            
            pub.CreatedAt = DateTime.Now;
            db.Publications.Add(pub);
            await db.SaveChangesAsync();
            return Results.Ok(pub);
        });

        app.MapDelete($"{Constants.Api.Publications}/{{id:int}}", async (HttpContext context, AppDbContext db, int id) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();

            var pub = await db.Publications.FindAsync(id);
            if (pub == null) return Results.NotFound();
            db.Publications.Remove(pub);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
