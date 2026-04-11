using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace MyWebTemplateV2.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private string? _token;
        private const string TOKEN_KEY = "fpt_nexus_auth_token";

        public bool IsLoggedIn => !string.IsNullOrEmpty(_token);
        public string? Token => _token;

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        public async Task InitializeAsync()
        {
            try {
                _token = await _js.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);
            } catch { }
        }

        public async Task<bool> VerifyAdminAsync(string username, string password)
        {
            // Endpoint is fixed as /api/admin/verify per requirement to avoid Constants dependency if not strictly needed in Client
            var response = await _http.PostAsJsonAsync("/api/admin/verify", new { Username = username, Password = password });
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                _token = result?.Token;
                if (!string.IsNullOrEmpty(_token))
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, _token);
                }
                return true;
            }
            
            return false;
        }

        public async Task Logout() 
        {
            _token = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
        }

        private class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}
