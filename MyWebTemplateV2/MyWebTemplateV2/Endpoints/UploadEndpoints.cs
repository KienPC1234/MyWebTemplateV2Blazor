using MyWebTemplateV2.Services;

namespace MyWebTemplateV2.Endpoints;

public static class UploadEndpoints
{
    public static void MapUploadEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/upload", async (HttpContext context, FileUploadService uploadService) => {
            if (!ApiSecurity.IsAuthorized(context)) return Results.Unauthorized();
            
            var form = await context.Request.ReadFormAsync();
            var file = form.Files.GetFile("file");
            var subFolder = form["subFolder"].ToString() ?? "general";

            if (file == null) return Results.BadRequest("No file uploaded.");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Results.Ok(new { Path = $"/uploads/{subFolder}/{fileName}" });
        });
    }
}
