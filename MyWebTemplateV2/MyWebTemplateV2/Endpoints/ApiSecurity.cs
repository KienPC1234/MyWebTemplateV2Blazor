namespace MyWebTemplateV2.Endpoints;

public static class ApiSecurity
{
    public static bool IsAuthorized(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        return authHeader == "FPT-NEXUS-ADMIN-SECRET-TOKEN"; 
    }
}
