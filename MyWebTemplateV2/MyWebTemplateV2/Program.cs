using MyWebTemplateV2.Components;
using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Services;
using MyWebTemplateV2.Client.Models;
using NeoUI.Blazor;
using NeoUI.Blazor.Extensions;
using NeoUI.Blazor.Primitives.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var systemConnectionString = builder.Configuration.GetConnectionString("SystemConnection");
builder.Services.AddDbContext<SystemDbContext>(options =>
    options.UseMySql(systemConnectionString, ServerVersion.AutoDetect(systemConnectionString)));

builder.Services.AddSingleton<AiChatService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3002") });
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddNeoUIPrimitives();
builder.Services.AddNeoUIComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

// API: Chat
app.MapPost("/api/chat", async (HttpContext context, AiChatService aiChatService) =>
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

// API: Submissions
app.MapGet("/api/submissions", async (AppDbContext db) => 
    await db.Submissions.OrderByDescending(s => s.CreatedAt).ToListAsync());

app.MapPost("/api/submissions", async (AppDbContext db, Submission submission) => {
    submission.CreatedAt = DateTime.Now;
    db.Submissions.Add(submission);
    await db.SaveChangesAsync();
    return Results.Created($"/api/submissions/{submission.Id}", submission);
});

// API: Publications
app.MapGet("/api/publications", async (AppDbContext db) => 
    await db.Publications.OrderByDescending(p => p.CreatedAt).Take(6).ToListAsync());

app.MapPost("/api/publications", async (AppDbContext db, Publication pub) => {
    pub.CreatedAt = DateTime.Now;
    db.Publications.Add(pub);
    await db.SaveChangesAsync();
    return Results.Ok(pub);
});

// API: AI Knowledge
app.MapGet("/api/ai-knowledge", async (SystemDbContext db) => 
    await db.AiKnowledge.FirstOrDefaultAsync() ?? new AiKnowledge { Context = "Bạn là trợ lý ảo của website Ngoại khóa nhịp đập." });

app.MapPost("/api/ai-knowledge", async (SystemDbContext db, AiKnowledge knowledge) => {
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

app.MapPost("/api/admin/verify", async (AuthService auth, AdminLoginRequest req) => 
    await auth.VerifyAdminAsync(req.Username, req.Password) ? Results.Ok() : Results.Unauthorized());

app.MapDelete("/api/publications/{id:int}", async (AppDbContext db, int id) => {
    var pub = await db.Publications.FindAsync(id);
    if (pub == null) return Results.NotFound();
    db.Publications.Remove(pub);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/submissions/{id:int}", async (AppDbContext db, int id) => {
    var sub = await db.Submissions.FindAsync(id);
    if (sub == null) return Results.NotFound();
    db.Submissions.Remove(sub);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyWebTemplateV2.Client.Routes).Assembly);

// Ensure Databases and Initial Admin
using (var scope = app.Services.CreateScope())
{
    var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var systemDb = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    
    await appDb.Database.EnsureCreatedAsync();
    await systemDb.Database.EnsureCreatedAsync();

    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    await authService.CreateInitialAdminAsync("admin", "Fpt@123456");

    await systemDb.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS `AiKnowledge` (
            `Id` int NOT NULL AUTO_INCREMENT,
            `Context` longtext NOT NULL,
            `UpdatedAt` datetime(6) NOT NULL,
            PRIMARY KEY (`Id`)
        ) CHARACTER SET=utf8mb4;");

    if (!await systemDb.AiKnowledge.AnyAsync())
    {
        systemDb.AiKnowledge.Add(new AiKnowledge 
        { 
            Context = "Bạn là trợ lý ảo của website Ngoại khóa nhịp đập. Website này do FPT Education phát triển, chuyên cung cấp các hoạt động ngoại khóa như Cuộc thi sáng tác Văn học, Ấn phẩm Nhái Bén và các hoạt động KTPL. Bạn hãy luôn hỗ trợ học sinh với thái độ thân thiện, hào hứng và chuyên nghiệp nhất." 
        });
        await systemDb.SaveChangesAsync();
    }
}

app.Run();

public record ChatRequest(string Prompt, IEnumerable<string>? Images = null);
public record AdminLoginRequest(string Username, string Password);
