namespace MinishCap;

public static class Fade {
    public static void InitFade() {
        Globals.FadeControl = new() {
            Mask = 0xFFFFFFFF
        };

        Array.Clear(Globals.Unk020354C0);
    }
}