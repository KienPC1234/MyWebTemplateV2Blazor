using MyWebTemplateV2.Services;
using MyWebTemplateV2.Client;

namespace MyWebTemplateV2.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(Constants.Api.AdminVerify, async (AuthService auth, AdminLoginRequest req) => {
            if (await auth.VerifyAdminAsync(req.Username, req.Password)) {
                return Results.Ok(new { Token = "FPT-NEXUS-ADMIN-SECRET-TOKEN" });
            }
            return Results.Unauthorized();
        });
    }
}

public record AdminLoginRequest(string Username, string Password);
