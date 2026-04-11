using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Client;
using Microsoft.AspNetCore.SignalR;
using MyWebTemplateV2.Hubs;

namespace MyWebTemplateV2.Endpoints;

public static class InteractionEndpoints
{
    public static void MapInteractionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost($"{Constants.Api.Submissions}/{{id:int}}/vote", async (AppDbContext db, int id, HttpContext context, IHubContext<NotificationHub> hubContext) => {
            var sub = await db.Submissions.FindAsync(id);
            if (sub == null) return Results.NotFound();

            // Use client IP as a simple fingerprint for demo purposes
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            var existingVote = await db.Votes.FirstOrDefaultAsync(v => v.SubmissionId == id && v.UserFingerprint == ip);
            if (existingVote != null) {
                return Results.BadRequest("Bạn đã bình chọn cho tác phẩm này rồi.");
            }

            db.Votes.Add(new Vote { SubmissionId = id, UserFingerprint = ip, Value = 1 });
            await db.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync("SubmissionUpdated");

            return Results.Ok(new { Message = "Đã bình chọn thành công." });
        });

        app.MapPost($"{Constants.Api.Submissions}/{{id:int}}/comment", async (AppDbContext db, int id, Comment comment, IHubContext<NotificationHub> hubContext) => {
            var sub = await db.Submissions.FindAsync(id);
            if (sub == null) return Results.NotFound();

            if (string.IsNullOrWhiteSpace(comment.Text))
                return Results.BadRequest("Nội dung bình luận không được để trống.");

            comment.SubmissionId = id;
            comment.CreatedAt = DateTime.Now;
            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            await hubContext.Clients.All.SendAsync("SubmissionUpdated");

            return Results.Ok(comment);
        });

        app.MapGet($"{Constants.Api.Submissions}/comments", async (AppDbContext db) => {
            return await db.Comments.OrderByDescending(c => c.CreatedAt).ToListAsync();
        });

        app.MapDelete($"{Constants.Api.Submissions}/comment/{{id:int}}", async (HttpContext context, AppDbContext db, int id) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();

            var cmt = await db.Comments.FindAsync(id);
            if (cmt == null) return Results.NotFound();
            db.Comments.Remove(cmt);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
