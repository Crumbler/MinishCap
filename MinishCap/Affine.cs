using MinishCap.Extensions;
using MinishCap.Structures;
using System.Runtime.InteropServices;

namespace MinishCap;

public static class Affine {
    public static void CopyOAM() {
        if (Globals.Main.Pad == 0) {
            Globals.OamControls.Unk[0x20].Unk0 = 0;
            Globals.OamControls.Unk[0x48].Unk4 = 0;
            Globals.OamControls.Unk[0x71].Unk0 = 0;
            Globals.OamControls.Unk[0x99].Unk4 = 0;
        } else {
            Globals.Main.Pad--;
        }

        ResetOamData();

        if (Globals.OamControls.Unk[0].Unk7) {
            Globals.OamControls.Unk[0].Unk7 = false;
            ObjAffineSet(32, 8);
        }

        Globals.OamControls.Field0x0 = 1;
    }

    private static void ObjAffineSet(int count, int offset) {
        var objects = Globals.OamControls.Unk.AsSpanOf<OamObj, ObjAffineSourceData>()[..count];

        // Base offset to affineParam element in shorts
        const int affineParamOffset = 3;

        var shortOffset = offset / sizeof(short);

        var matrixData = Globals.OamControls.Oam.AsSpanOf<OamData, short>()[affineParamOffset..];
        for (int i = 0; i < objects.Length; ++i) {
            var obj = objects[i];

            float sx = obj.XScale >>> 8;
            float sy = obj.YScale >>> 8;
            float theta = (obj.Rotation >>> 8) / 128f * MathF.PI;

            float a, b, c, d;
            a = d = float.Cos(theta);
            b = c = float.Sin(theta);

            a *= sx * 256;
            b *= -sx * 256;
            c *= sy * 256;
            d *= sy * 256;

            int baseIndex = i * shortOffset * 4;
            matrixData[baseIndex] = (short)a;
            matrixData[baseIndex + shortOffset] = (short)b;
            matrixData[baseIndex + shortOffset * 2] = (short)c;
            matrixData[baseIndex + shortOffset * 3] = (short)d;
        }
    }

    private static void ResetOamData() {
        var oamData = Globals.OamControls.Oam.AsSpan(..Globals.OamControls.Updated);
        var data = MemoryMarshal.Cast<OamData, ushort>(oamData);

        // OamData is 8 bytes so we increment the index by 4
        for (int i = 0; i < data.Length; i += 4) {
            data[i] = 0x2A0;
        }
    }

    public static void FlushSprites() {
        Globals.OamControls.Updated = 0;
    }
}