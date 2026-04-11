namespace MyWebTemplateV2.Endpoints;

public static class ApiEndpoints
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapSubmissionEndpoints();
        app.MapPublicationEndpoints();
        app.MapAiEndpoints();
        app.MapUploadEndpoints();
        app.MapInteractionEndpoints();
        app.MapSettingsEndpoints();
    }
}
