using MyWebTemplateV2.Components;
using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Services;
using MyWebTemplateV2.Extensions;
using MyWebTemplateV2.Endpoints;
using MyWebTemplateV2.Client;
using MyWebTemplateV2.Client.Models;
using MyWebTemplateV2.Hubs;
using MyWebTemplateV2; // Added this

var builder = WebApplication.CreateBuilder(args);

// Add services to the container using extension method
builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

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

// Map API Endpoints using extension method
app.MapApiEndpoints();
app.MapHub<NotificationHub>("/notificationHub");

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
    await authService.CreateInitialAdminAsync(Constants.Admin.DefaultUsername, Constants.Admin.DefaultPassword);

    // Seed Data for Publications and Submissions if empty
    if (!await appDb.Publications.AnyAsync())
    {
        appDb.Publications.AddRange(
            new Publication { Title = "Nhái Bén #1: Khởi Nguyên", Author = "Ban biên tập", Description = "Số báo đầu tiên của ấn phẩm Nhái Bén với nhiều tâm sự của lứa học sinh khóa 1.", CoverImagePath = "images/dashboard_mockup.png", PdfPath = "#", CreatedAt = DateTime.Now.AddDays(-30) },
            new Publication { Title = "Nhái Bén #2: Thanh Xuân", Author = "Ban biên tập", Description = "Ghi lại những khoảnh khắc đẹp nhất của tuổi học trò.", CoverImagePath = "images/dashboard_mockup.png", PdfPath = "#", CreatedAt = DateTime.Now.AddDays(-15) },
            new Publication { Title = "Nhái Bén #3: Tương Lai", Author = "Ban biên tập", Description = "Những ước mơ và hoài bão của học sinh FPT.", CoverImagePath = "images/dashboard_mockup.png", PdfPath = "#", CreatedAt = DateTime.Now }
        );
        await appDb.SaveChangesAsync();
    }

    if (!await appDb.Submissions.AnyAsync())
    {
        appDb.Submissions.AddRange(
            new Submission { Title = "Bài văn đạt giải Nhất thành phố", Content = "Đây là nội dung bài văn...", Author = "Nguyễn Văn A", Type = SubmissionType.SangTac, Subject = SubjectArea.Van, CreatedAt = DateTime.Now.AddDays(-10) },
            new Submission { Title = "Tài liệu ôn thi KTPL giữa kì", Content = "Tóm tắt các kiến thức trọng tâm...", Author = "Trần Thị B", Type = SubmissionType.BaiThi, Subject = SubjectArea.KTPL, CreatedAt = DateTime.Now.AddDays(-5) },
            new Submission { Title = "Cảm nhận về mùa thu Hà Nội", Content = "Mùa thu Hà Nội thật đẹp...", Author = "Lê Văn C", Type = SubmissionType.SangTac, Subject = SubjectArea.Van, CreatedAt = DateTime.Now.AddDays(-2) },
            new Submission { Title = "Đề thi thử môn Văn học kì 2", Content = "Bộ đề thi thử bám sát cấu trúc đề minh họa...", Author = "Giáo viên D", Type = SubmissionType.BaiThi, Subject = SubjectArea.Van, CreatedAt = DateTime.Now }
        );
        await appDb.SaveChangesAsync();
    }

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
