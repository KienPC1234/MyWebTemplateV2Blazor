using System.Net.Http.Json;

namespace MyWebTemplateV2.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        public bool IsLoggedIn { get; private set; }

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> VerifyAdminAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/admin/verify", new { Username = username, Password = password });
            IsLoggedIn = response.IsSuccessStatusCode;
            return IsLoggedIn;
        }

        public void Logout() => IsLoggedIn = false;
    }
}
