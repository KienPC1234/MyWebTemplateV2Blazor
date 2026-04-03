# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MyWebTemplateV2** is a Blazor WebAssembly + Interactive WebAssembly hybrid app built on .NET 10.0. It serves as a UI component library styled with Tailwind CSS using a Zinc Monochrome / Glassmorphism design language.

The solution has two projects:
- **Server**: `MyWebTemplateV2/` (this directory) — ASP.NET Core host with Razor Components
- **Client**: `../MyWebTemplateV2.Client/` — Blazor WebAssembly client project

## Architecture

### Component Organization

UI components live in `Components/UI/` and are organized into 13 numbered groups:

| Group | Folder | Purpose |
|-------|--------|---------|
| Group 1 | `Group1_Layout/` | Scaffolding: `SaaSMainShell`, `GlassNavbar`, `BentoGrid`, `Card`, `Container`, `AcrylicSidebar` |
| Group 2 | `Group2_Actions/` | Buttons & triggers: `SaaSButton`, `CommandPalette`, `GlassIconButton`, `DoubleAction`, `BackLink` |
| Group 3 | `Group3_DataDisplay/` | Data display: `SaaSTable`, `StatsCard`, `StatusBadge`, `ActivityFeed`, `AvatarStack`, `EmptyState` |
| Group 4 | `Group4_Forms/` | Form inputs: `MinimalInput`, `GlassTextArea`, `SaaSSelect`, `ModernSwitch`, `FieldLabel`, `CheckboxCustom` |
| Group 5 | `Group5_Overlays/` | Overlays: `GlassModal`, `SaaSToast`, `MinimalTooltip`, `SkeletonPulse`, `ContextDropdown` |
| Group 6 | `Group6_Navigation/` | Navigation: `SaaSTabs`, `Stepper`, `Breadcrumb`, `Pagination` |
| Group 7 | `Group7_Specialized/` | Specialized: `UserSnippet`, `CopyButton`, `GlassDivider`, `KbdKey` |
| Group 8 | `Group8_Immersive/` | Immersive: `MeshGradientCanvas`, `GlassParallaxStack`, `EdgeBlur` |
| Group 9 | `Group9_AI/` | AI interfaces: `AIPromptConsole`, `ThoughtBubble`, `TheCommandBar`, `MagicActionPill` |
| Group 10 | `Group10_Advanced/` | Advanced: `GlassCodeSnippet`, `FloatingIsland`, `MagneticButton`, `DragDropGlassZone`, `KbdShortcutHint` |
| Group 11 | `Group11_FutureData/` | Futuristic charts: `GlassGlobe`, `HolographicBarChart`, `LiveOrbit`, `HeatmapGlass` |
| Group 12 | `Group12_Interactions/` | Micro-interactions: `SoftToggle`, `GlassSlider`, `ProgressGlassBar`, `StaggeredList`, `ConfettiGlass` |
| Group 13 | `Group13_Entry/` | Entry effects: `InitialLoader`, `SkeletonGlass`, `PageTransition` |

Key layout files:
- `Components/Layout/MainLayout.razor` — wraps content in `SaaSMainShell` with `GlassNavbar`, nav links to `/` (Dashboard) and `/ui-kit` (UI Kit)
- `Components/Pages/Home.razor` (`@page "/"`) — component showcase dashboard with tabbed group views
- `Components/Pages/SaaSComponentsManifest.razor` (`@page "/ui-kit"`) — UI kit reference page

### Styling Pipeline

- **Tailwind CSS 3.4** with custom Zinc color palette (see `tailwind.config.js`)
- Source: `Styles/tailwind.css` → compiled to `wwwroot/app.css`
- Custom utility classes: `.glass`, `.acrylic`, `.btn-saas-*`, `.saas-content`, `.noise-overlay`
- Dark mode via `class` strategy (toggled by `ThemeService`)

### Theme System

`ThemeService` (`Components/UI/ThemeService.cs`) is a scoped singleton that toggles dark/light themes via JS interop (`themeManager.getTheme` / `themeManager.setTheme`). The JS-side `themeManager` object must be defined in the host page.

## Common Commands

### Build & Run the App

```bash
dotnet run
```

### Tailwind CSS

```bash
# One-time build
npm run build

# Watch mode (auto-recompile on file changes)
npm run watch

# Or use the helper batch file on Windows
build-tailwind.bat
```

### Full Rebuild

If Tailwind CSS hasn't been compiled yet, install dependencies and build:

```bash
npm install
npm run build
```

## Important Notes

- The app uses `.razor` component syntax with `@code` blocks and child content patterns (`<ChildContent>`, `<Header>`, `<Footer>`, etc.)
- Components follow the naming convention `SaaS`, `Glass`, `Minimal`, `Soft` prefixes
- Use Zinc color scale (`zinc-50` through `zinc-950`) to maintain monochrome consistency — refer to `tailwind.config.js` custom colors (`surface-*`, `primary-*`)
- The server project has a `Components/UI/ThemeService.cs` that mirrors the client `Services/ThemeService.cs` — both are used for theme toggling via JS interop
