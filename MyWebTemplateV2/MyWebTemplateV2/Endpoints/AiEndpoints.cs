using MyWebTemplateV2.Data;
using MyWebTemplateV2.Services;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Client;
using Microsoft.EntityFrameworkCore;

namespace MyWebTemplateV2.Endpoints;

public static class AiEndpoints
{
    public static void MapAiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Constants.Api.Chat, async (HttpContext context, AiChatService aiChatService) =>
        {
            var request = await context.Request.ReadFromJsonAsync<ChatRequest>();
            if (request == null || string.IsNullOrEmpty(request.Prompt)) return;
            
            context.Response.ContentType = "text/plain";
            try {
                await foreach (var token in aiChatService.StreamChatAsync(request.Prompt, request.Images))
                {
                    await context.Response.WriteAsync(token);
                    await context.Response.Body.FlushAsync();
                }
            } catch (Exception ex) {
                await context.Response.WriteAsync($"[System Error]: {ex.Message}");
            }
        });

        app.MapGet(Constants.Api.AiKnowledge, async (SystemDbContext db) => 
            await db.AiKnowledge.FirstOrDefaultAsync() ?? new AiKnowledge { Context = "Bạn là trợ lý ảo của website Ngoại khóa nhịp đập." });

        app.MapPost(Constants.Api.AiKnowledge, async (HttpContext context, SystemDbContext db, AiKnowledge knowledge) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();

            var existing = await db.AiKnowledge.FirstOrDefaultAsync();
            if (existing == null) {
                db.AiKnowledge.Add(knowledge);
            } else {
                existing.Context = knowledge.Context;
                existing.UpdatedAt = DateTime.Now;
            }
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}

public record ChatRequest(string Prompt, IEnumerable<string>? Images = null);
