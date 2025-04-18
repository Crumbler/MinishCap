using MinishCap.Extensions;
using MinishCap.Structures;

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
        Globals.Screen.Bg0.Updated = refresh;
    }

    private static void ClearOAM() {
        var oamData = Globals.OamControls.Oam.AsSpanOf<OamData, ushort>();
        var oam = Buffers.Oam.AsSpanOf<byte, ushort>();

        int ind = 0;

        // Reset first 2 bytes of each OAM object
        for (int i = 0; i < 128; ++i) {
            oamData[ind] = 0x2A0;
            oam[ind] = 0x2A0;

            ind += 4;
        }
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

    public static void Sub0801E1EC(uint a1, uint a2, uint a3) {
        // Do stuff
    }

    public static void Sub0801E104() {
        unchecked {
            Globals.Screen.Lcd.DisplayControl &= (ushort)~0x6000;
        }

        Globals.Screen.VBlankDMA.Ready = false;
    }
}