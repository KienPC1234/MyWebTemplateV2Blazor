using System;
using System.Reflection;
using System.Linq;

try {
    var assembly = Assembly.LoadFrom("/home/kien/.nuget/packages/lucide.blazor/0.0.38/lib/net8.0/Lucide.Blazor.dll");
    var types = assembly.GetTypes().Where(t => t.IsPublic && !t.IsAbstract);
    foreach (var type in types.Take(50)) {
        Console.WriteLine(type.FullName);
    }
} catch (Exception ex) {
    Console.WriteLine(ex.Message);
}
