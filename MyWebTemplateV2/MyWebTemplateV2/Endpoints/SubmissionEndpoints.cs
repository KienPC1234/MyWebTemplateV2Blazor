using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Client;
using MyWebTemplateV2.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MyWebTemplateV2.Endpoints;

public static class SubmissionEndpoints
{
    public static void MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(Constants.Api.Submissions, async (AppDbContext db) => 
            await db.Submissions.Include(s => s.Votes).Include(s => s.Comments).OrderByDescending(s => s.CreatedAt).ToListAsync());

        app.MapPost(Constants.Api.Submissions, async (AppDbContext db, Submission submission, IHubContext<NotificationHub> hubContext) => {
            if (string.IsNullOrWhiteSpace(submission.Title) || string.IsNullOrWhiteSpace(submission.Content))
                return Results.BadRequest("Title and Content are required.");

            submission.CreatedAt = DateTime.Now;
            db.Submissions.Add(submission);
            await db.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync("SubmissionUpdated");

            return Results.Created($"/api/submissions/{submission.Id}", submission);
        });

        app.MapDelete($"{Constants.Api.Submissions}/{{id:int}}", async (HttpContext context, AppDbContext db, int id) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();

            var sub = await db.Submissions.FindAsync(id);
            if (sub == null) return Results.NotFound();
            db.Submissions.Remove(sub);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
