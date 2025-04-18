using MinishCap.Structures;

namespace MinishCap;

[Flags]
public enum FadeType : ushort {
    None = 0,
    InOut = 1,
    BlackWhite = 2,
    Instant = 4,
    Mosaic = 8,
    Iris = 16
}

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

    public static void SetFade(FadeType type, uint speed) {
        Globals.FadeControl.Speed = (ushort)speed;
        Globals.FadeControl.Type = type;
        Globals.FadeControl.Active = true;
        Globals.FadeControl.Progress = 0x100;
        Globals.FadeControl.Sustain = 0;

        if (type.HasFlag(FadeType.BlackWhite)) {
            Globals.FadeControl.Color = 0xF8;
        } else {
            Globals.FadeControl.Color = 0;
        }

        if (type.HasFlag(FadeType.Mosaic)) {
            Globals.OamControls.SpritesOffset = 1;
            Globals.Screen.Bg1.Control |= Constants.BackgroundControl.Mosaic;
            Globals.Screen.Bg2.Control |= Constants.BackgroundControl.Mosaic;
            Globals.Screen.Bg3.Control |= Constants.BackgroundControl.Mosaic;
        }

        if (type.HasFlag(FadeType.Iris)) {
            if (!type.HasFlag(FadeType.InOut)) {
                Globals.FadeControl.Type &= ~FadeType.Instant;
                ResetFadeMask();
                // Globals.UsedPalettes = 0xFFFFFFFF; Already done in ResetFadeMask
            }
        }
    }

    private static void ResetFadeMask() {
        Array.Clear(Globals.Unk020354C0);
        Globals.UsedPalettes = 0xFFFFFFFF;
    }

    public static void FadeMain() {
        ref FadeControl ctrl = ref Globals.FadeControl;

        var flags = ctrl.Type & (FadeType.Instant | FadeType.Mosaic | FadeType.Iris);

        if (ctrl.Active) {
            ctrl.Progress -= ctrl.Speed;

            if ((short)ctrl.Progress <= (short)ctrl.Sustain) {
                ctrl.Progress = ctrl.Sustain;
            }

            bool active = false;

            while (flags != FadeType.None) {
                var bit = (~flags + 1) & flags;
                flags ^= bit;
                switch (bit) {
                    case FadeType.Instant:
                        active |= HandleInstantFade(ctrl);
                        break;

                    case FadeType.Mosaic:
                        active |= HandleMosaicFade(ctrl);
                        break;

                    case FadeType.Iris:
                        active |= HandleIrisFade(ctrl);
                        break;
                }
            }

            ctrl.Active = active;
        }
    }

    private static bool HandleInstantFade(in FadeControl ctrl) {
        uint v1;

        if (ctrl.Type.HasFlag(FadeType.InOut)) {
            v1 = (uint)(256 - ctrl.Progress);
        } else {
            v1 = ctrl.Progress;
        }

        uint v2 = ctrl.Mask;

        foreach (ref var unk in Globals.Unk020354C0.AsSpan()) {
            unk.Unk0 = (byte)(v2 & 1);
            unk.Unk2 = (v2 & 1) == 0 ? (ushort)0 : (ushort)v1;
        }

        Globals.UsedPalettes = 0xFFFFFFFF;

        return (ctrl.Sustain ^ ctrl.Progress) != 0;
    }

    private static readonly ushort[] MosaicSizes = [
        0,      0x1111, 0x2222, 0x3333, 0x4444, 0x5555, 0x6666, 0x7777,
        0x8888, 0x9999, 0xaaaa, 0xbbbb, 0xcccc, 0xdddd, 0xeeee, 0xffff,
    ];

    private static bool HandleMosaicFade(in FadeControl ctrl) {
        var type = ctrl.Type;

        uint idx = (uint)((((short)ctrl.Progress) >> 4) & 0xF);

        if (type.HasFlag(FadeType.InOut)) {
            idx = 0xF - idx;
        }

        Globals.Screen.Controls.MosaicSize = MosaicSizes[idx];

        if (ctrl.Progress != 0) {
            return true;
        }

        // Fade is finished
        Globals.OamControls.SpritesOffset = 0;

        if (!type.HasFlag(FadeType.InOut)) {
            ushort antiMosaicMask = Constants.BackgroundControl.Mosaic;
            antiMosaicMask = (ushort)~antiMosaicMask;

            Globals.Screen.Bg0.Control &= antiMosaicMask;
            Globals.Screen.Bg1.Control &= antiMosaicMask;
            Globals.Screen.Bg2.Control &= antiMosaicMask;
            Globals.Screen.Bg3.Control &= antiMosaicMask;
        }

        return false;
    }

    private static bool HandleIrisFade(in FadeControl ctrl) {
        if (ctrl.Type.HasFlag(FadeType.InOut)) {
            int delta = (ushort)Globals.FadeControl.IrisSize - Globals.FadeControl.Speed;
            Globals.FadeControl.IrisSize -= (short)Globals.FadeControl.Speed;

            if ((delta << 16) <= 0) {
                Globals.FadeControl.IrisSize = 0;
            }

            Common.Sub0801E1EC(
                (uint)Globals.FadeControl.IrisX,
                (uint)Globals.FadeControl.IrisY,
                (uint)Globals.FadeControl.IrisSize);

            if (Globals.FadeControl.IrisSize == 0) {
                return false;
            }
        } else {
            Globals.FadeControl.IrisSize += (short)Globals.FadeControl.Speed;

            Common.Sub0801E1EC(
                (uint)Globals.FadeControl.IrisX,
                (uint)Globals.FadeControl.IrisY,
                (uint)Globals.FadeControl.IrisSize);

            if (Globals.FadeControl.IrisSize > 150) {
                Common.Sub0801E104();
                return false;
            }
        }

        return true;
    }
}