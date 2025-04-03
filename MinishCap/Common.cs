namespace MinishCap;

public static class Common {
    public static void DisplayReset(bool refresh) {
        Globals.Main.InterruptFlag = 1;
        Globals.Unk03003DE0 = 0;
        Globals.FadeControl.Active = false;
        Globals.Screen.VBlankDMA.ReadyBackup = false;
        Globals.Screen.VBlankDMA.Ready = false;
        ClearOAM();
        ResetScreenRegs();

        // Clear 32 bytes at 0x600C000 (VRAM)
        Array.Clear(Buffers.BG0);
        Globals.Screen.Bg0.Updated = 0;
    }

    private static void ClearOAM() {

    }

    private static void ResetScreenRegs() {
        Globals.Screen = new();
        Globals.Screen.Bg0.SubTilemap = Buffers.BG0;
        Globals.Screen.Bg0.Control = 0x1F0C;
        Globals.Screen.Bg1.SubTilemap = Buffers.BG1;
        Globals.Screen.Bg1.Control = 0x1C01;
        Globals.Screen.Bg2.SubTilemap = Buffers.BG2;
        Globals.Screen.Bg2.Control = 0x1D02;
        Globals.Screen.Bg3.SubTilemap = Buffers.BG3;
        Globals.Screen.Bg3.Control = 0x1E01;
        Globals.Screen.Lcd.DisplayControl = 0x140;
        Globals.Screen.Lcd.DisplayControlMask = 0xFFFF;
    }
}