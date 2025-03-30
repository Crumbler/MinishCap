using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace MinishCap; 

public static class Program {
    private static IWindow _window = null!;

    public static void Main() {
        Console.WriteLine("Hello, World!");

        var options = WindowOptions.Default with {
            Size = new Vector2D<int>(800, 600),
            Title = "Minish Cap",
            FramesPerSecond = 60
        };

        _window = Window.Create(options);

        _window.Run();
    }
}
