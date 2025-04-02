using MinishCap.Helpers;

namespace MinishCap;

public static class Messages {
    private struct Window {
        public byte Unk0, Active, Unk2, Unk3,
            XPos, YPos, Width, Height;
    }

    private static Window CurrentWindow, NewWindow;

    public static void MessageInitialize() {
        Globals.Message = new();
        Globals.TextRender = new();
        CurrentWindow = new();
        NewWindow = new();
        StructHelper.AsByteSpan(ref Globals.Unk02000040)[..4].Clear();
    }
}