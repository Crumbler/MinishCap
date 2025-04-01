using MinishCap.Enums;
using System.Runtime.CompilerServices;

namespace MinishCap.Structures;

public struct StructMain {
    public Tasks CurrentTask;
    public TaskStates CurrentTaskState;
    public TaskSubStates CurrentTaskSubState;
    public byte MainField0x5;
    public byte MainField0x7;
    public ushort Ticks;
}

public unsafe struct Struct02000010 {
    public int Signature;
    public byte Field0x4;
    public byte ListenForKeyPresses;
    public byte Field0x6;
    public byte Field0x7;
    public fixed byte Padding[24];
}

public unsafe struct SaveHeader {
    private const int NameCount = 6;

    public int Signature;
    public byte SaveFileId;
    public byte MsgSpeed;
    public byte Brightness;
    public Languages Language;
    public fixed byte Name[NameCount];
    public byte Invalid;
    public byte Initialized;

    public Span<byte> NameSpan {
        get {
            var ptr = Unsafe.AsPointer(ref Name[0]);
            return new Span<byte>(ptr, NameCount);
        }
    }
}

public struct FadeControl {
    /// <summary>
    ///     Currently fading
    /// </summary>
    public bool Active;

    public byte Unused1, Color, Unused2;

    /// <summary>
    ///     Fade palette mask.
    ///     LSB = foreground, MSB = background.
    /// </summary>
    public uint Mask;
    public ushort Type, Speed, Progress;

    /// <summary>
    ///     Fade progress to sustain
    /// </summary>
    public ushort Sustain;
    public short IrisSize, IrisX, IrisY;
    public ushort WinInsideCount, WinOutsideCount;
}

public struct Struct020354C0 {
    public byte Unk0, Unk1;
    public ushort Unk2;
}

public struct Message {
    public byte State, Unk, TextSpeed, Unk3,
        TextWindowWidth, TextWindowHeight,
        TextWindowPosX, TextWindowPosY;

    public ushort TextIndex, Unk2;

    public uint Flags, Rupees;

    public uint Field0x14, Field0x18, Field0x1C;
}