using Microsoft.JSInterop;

namespace MyWebTemplateV2.Client.Components.UI;

public class ThemeService
{
    private readonly IJSRuntime _js;
    public string CurrentTheme { get; private set; } = "light";
    
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        CurrentTheme = await _js.InvokeAsync<string>("themeManager.getTheme");
        NotifyChanged();
    }

    public async Task ToggleThemeAsync()
    {
        CurrentTheme = CurrentTheme == "light" ? "dark" : "light";
        await _js.InvokeVoidAsync("themeManager.setTheme", CurrentTheme);
        NotifyChanged();
    }

    private void NotifyChanged() => OnThemeChanged?.Invoke();
}
