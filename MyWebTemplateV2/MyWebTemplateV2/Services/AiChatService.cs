using OllamaSharp;
using OllamaSharp.Models.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MyWebTemplateV2.Services
{
    public class AiChatService
    {
        private readonly OllamaApiClient _ollamaClient;
        private readonly string _modelName = "gemma4:31b-cloud";
        private readonly IServiceScopeFactory _scopeFactory;

        public AiChatService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            var uri = new Uri(configuration["Ollama:Uri"] ?? "http://localhost:11434");
            _ollamaClient = new OllamaApiClient(uri);
            _ollamaClient.SelectedModel = _modelName;
            _scopeFactory = scopeFactory;
        }

        public async IAsyncEnumerable<string> StreamChatAsync(string prompt, IEnumerable<string>? base64Images = null)
        {
            string systemPrompt = "Bạn là trợ lý ảo của website Ngoại khóa nhịp đập. Hãy trả lời thân thiện và chuyên nghiệp.";
            
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Data.SystemDbContext>();
                var knowledge = await db.AiKnowledge.FirstOrDefaultAsync();
                if (knowledge != null && !string.IsNullOrEmpty(knowledge.Context))
                {
                    systemPrompt = knowledge.Context;
                }
            }

            Chat? chat = null;
            string? initError = null;
            try {
                chat = new Chat(_ollamaClient, systemPrompt);
            } catch (Exception ex) {
                initError = $"[Error Initializing AI]: {ex.Message}. Please ensure Ollama is running.";
            }
            
            if (initError != null) {
                yield return initError;
                yield break;
            }
            
            IEnumerable<byte[]>? images = null;
            if (base64Images != null)
            {
                images = base64Images.Select(Convert.FromBase64String);
            }
            
            IAsyncEnumerator<string>? enumerator = null;
            string? connError = null;
            try {
                enumerator = chat!.SendAsync(prompt, images).GetAsyncEnumerator();
            } catch (Exception ex) {
                connError = $"[Connection Error]: {ex.Message}";
            }

            if (connError != null) {
                yield return connError;
                yield break;
            }

            if (enumerator != null) {
                while (true) {
                    string? token = null;
                    string? streamError = null;
                    try {
                        if (!await enumerator.MoveNextAsync()) break;
                        token = enumerator.Current;
                    } catch (Exception ex) {
                        streamError = $" [Stream Interrupted]: {ex.Message}";
                    }
                    
                    if (streamError != null) {
                        yield return streamError;
                        break;
                    }
                    
                    yield return token ?? string.Empty;
                }
            }
        }
    }
}
