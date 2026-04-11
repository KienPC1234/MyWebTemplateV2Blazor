using OllamaSharp;
using OllamaSharp.Models.Chat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;

namespace MyWebTemplateV2.Services
{
    public class AiChatService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OllamaApiClient _ollama;

        /* 
         * CRITICAL MANDATE: DO NOT CHANGE THE MODEL BELOW.
         * THE SYSTEM MUST USE gemma4:31b-cloud EXCLUSIVELY.
         * MODIFICATION OF THIS VALUE IS STRICTLY FORBIDDEN.
         */
        private const string REQUIRED_MODEL = "gemma4:31b-cloud";

        public AiChatService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _ollama = new OllamaApiClient("http://localhost:11434");
            _ollama.SelectedModel = REQUIRED_MODEL;
        }

        public async IAsyncEnumerable<string> StreamChatAsync(string prompt, IEnumerable<string>? images = null)
        {
            string systemContext = "Bạn là trợ lý ảo của website Ngoại khóa nhịp đập.";
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var systemDb = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
                var knowledge = await systemDb.AiKnowledge.FirstOrDefaultAsync();
                if (knowledge != null) systemContext = knowledge.Context;
            }

            var messages = new List<Message>
            {
                new Message { Role = "system", Content = systemContext },
                new Message { Role = "user", Content = prompt }
            };

            string streamError = "";
            
            IAsyncEnumerable<ChatResponseStream?>? stream = null;
            try {
                // Strictly ensuring model is set before stream
                _ollama.SelectedModel = REQUIRED_MODEL;
                stream = _ollama.ChatAsync(new ChatRequest { Messages = messages, Stream = true });
            } catch (Exception ex) {
                streamError = $"[Ollama Connection Error]: {ex.Message}";
            }

            if (!string.IsNullOrEmpty(streamError))
            {
                yield return streamError;
            }
            else if (stream != null)
            {
                await foreach (var response in stream)
                {
                    if (response?.Message?.Content != null)
                    {
                        yield return response.Message.Content;
                    }
                }
            }
        }
    }
}
