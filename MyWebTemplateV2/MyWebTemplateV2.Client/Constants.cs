namespace MyWebTemplateV2;

public static class Constants
{
    public static class Api
    {
        public const string BaseAddress = "http://localhost:3002";
        public const string Chat = "/api/chat";
        public const string Submissions = "/api/submissions";
        public const string Publications = "/api/publications";
        public const string AiKnowledge = "/api/ai-knowledge";
        public const string Admin = "/api/admin";
        public const string AdminVerify = "/api/admin/verify";
        public const string Comments = "/api/submissions/comments";
    }

    public static class Routes
    {
        public const string Home = "/";
        public const string NgoaiKhoa = "/ngoai-khoa";
        public const string MonVan = "/mon-hoc/van";
        public const string MonKTPL = "/mon-hoc/ktpl";
        public const string LienHe = "/lien-he";
        public const string AdminLogin = "/admin/login";
        public const string AdminDashboard = "/admin/dashboard";
    }

    public static class Admin
    {
        public const string DefaultUsername = "admin";
        public const string DefaultPassword = "Fpt@123456";
    }
}
