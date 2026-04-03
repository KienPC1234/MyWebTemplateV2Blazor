# 🌌 Modern SaaS UI Kit - Developer Guide

> [!NOTE]
> This library is built using **Blazor**, **Tailwind CSS**, and **Zinc Monochrome** aesthetics. It utilizes deep Glassmorphism and Backdrop Blurs for a premium feel.

---

## 🏗️ 1. Layout & Scaffolding (Bộ khung)
Components that define the structural hierarchy of your app.

### `SaaSMainShell`
The root container. Includes the grainy overlay and global font settings.
```razor
<SaaSMainShell>
    <!-- Your content here -->
</SaaSMainShell>
```

### `GlassNavbar`
Sticky top navigation with customizable branding.
- `BrandTitle`: String (Default: "ANTIGRAVITY")
```razor
<GlassNavbar BrandTitle="MY APP" />
```

---

## 🔘 2. Actions & Triggers (Tương tác)
Tactile elements for user input and flow control.

### `SaaSButton`
The primary interaction element.
- `Variant`: "primary", "secondary", "ghost"
- `Size`: "sm", "md", "lg"
```razor
<SaaSButton Variant="primary">Deploy Node</SaaSButton>
```

### `CommandPalette` (⌘K)
Spotlight-style search interface.
```razor
<CommandPalette @bind-Visible="showPalette" Placeholder="Search commands..." />
```

---

## 📊 3. Data Display (Hiển thị dữ liệu)
Crisp, minimalist data visualization.

### `SaaSTable`
Border-less table with glass containers.
```razor
<SaaSTable>
    <Header>
        <th>Metric</th>
        <th>Status</th>
    </Header>
    <ChildContent>
        <tr>
            <td>Uptime</td>
            <td><StatusBadge Status="active">ONLINE</StatusBadge></td>
        </tr>
    </ChildContent>
</SaaSTable>
```

---

## 🛸 11. Futuristic Data (Dữ liệu tương lai)
Advanced SVG-based visualizations.

### `GlassGlobe`
A 3D-effect CSS globe.
- `Value`: The central metric percentage.
- `Label`: The sub-text.
```razor
<GlassGlobe Value="99.4%" Label="UPTIME" />
```

---

## 🧪 12. Micro-Interactions (Tương tác nhỏ)
Skeuomorphic and glass-based interactive elements.

### `SoftToggle`
A fluid, glass-based switch.
```razor
<SoftToggle @bind-Value="isSyncActive" />
```

---

## 🛠️ Customization & Branding (Tùy chỉnh)
To remove the default "ANTIGRAVITY" branding, use the `BrandTitle` or `AppTitle` parameters provided in:
1. `GlassNavbar`
2. `InitialLoader`
3. `TheCommandBar`

> [!TIP]
> Always use `Zinc` colors (zinc-50 to zinc-950) to maintain the monochrome aesthetic consistency.
