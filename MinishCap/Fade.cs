namespace MinishCap;

public static class Fade {
    public static void InitFade() {
        Globals.FadeControl = new() {
            Mask = 0xFFFFFFFF
        };

        Array.Clear(Globals.Unk020354C0);
    }

    public static void SetBrightness(byte brightness) {
        Globals.SaveHeader.Brightness = brightness;
        Globals.UsedPalettes = 0xFFFFFFFF;
    }

    public static void FadeMain() {

    }
}