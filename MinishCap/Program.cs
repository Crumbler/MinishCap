using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace MinishCap; 

public static class Program {
    private static IWindow _window = null!;

    public static void Main() {
        Console.WriteLine("Starting up");

        var options = WindowOptions.Default with {
            Size = new Vector2D<int>(800, 600),
            Title = "Minish Cap",
            FramesPerSecond = 60,
            UpdatesPerSecond = 60,
        };

        _window = Window.Create(options);

        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;

        _window.Run();
    }

    private static void OnRender(double dTime) {
        MinishCap.Main.AgbRender();
    }

    private static void OnUpdate(double dTime) {
        MinishCap.Main.AgbUpdate();
    }

    private static void OnLoad() {
        Console.WriteLine("Window loaded");

        MinishCap.Main.AgbLoad();
    }
}
