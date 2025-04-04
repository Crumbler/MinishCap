namespace MinishCap;

public struct OAMCommand {
    public short X, Y;
    public ushort _4, _6, _8;
}

public struct ScreenData {
    public LcdControls Lcd;
    public BgSettings Bg0, Bg1;
    public BgAffSettings Bg2, Bg3;
    public BgControls Controls;
    public VBlankDMA VBlankDMA;
}

public struct LcdControls {
    public ushort DisplayControl, Unk4, DisplayControlMask;
}

public struct BgSettings {
    public ushort Control, XOffset, YOffset;

    public bool Updated;

    public Memory<ushort> SubTilemap;
}

public struct BgAffSettings {
    public ushort Control;
    public short XOffset, YOffset;
    public ushort Updated;

    public Memory<ushort> SubTilemap;
}

public struct BgTransformationSettings {
    public ushort Dx, Dmx, Dy, Dmy,
        XPointLeastSig, XPointMostSig,
        YPointLeastSig, YPointMostSig;
}

public struct BgControls {
    public BgTransformationSettings Bg2, Bg3;
    public ushort Window0HorizontalDimensions, Window1HorizontalDimension,
        Window0VerticalDimensions, Window1VerticalDimensions,
        WindowInsideControl, WindowOutsideControl, MosaicSize,
        LayerFXControl, AlphaBlend, LayerBrightness;
}

public unsafe struct VBlankDMA {
    public bool Ready, ReadyBackup;
    public ushort Unused;
    public ushort* Src, Dest;
    public uint Size;
}

public static class Screen {
    public static void Sub080ADA04() {

    }
}