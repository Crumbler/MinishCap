using MinishCap.Enums;
using MinishCap.Helpers;
using System.Runtime.InteropServices;

namespace MinishCap.Structures;

public struct StructMain {
    public byte InterruptFlag, SleepStatus;
    public Tasks CurrentTask;
    public TaskStates CurrentTaskState;
    public TaskSubStates CurrentTaskSubState;
    public byte MainField0x5;
    public byte MuteAudio;
    public byte MainField0x7;

    /// <summary>
    ///     Number of frames to pause
    /// </summary>
    public byte PauseFrames;

    /// <summary>
    ///     Numbers of pauses to make
    /// </summary>
    public byte PauseCount;

    /// <summary>
    ///     Number of frames to play between each pause
    /// </summary>
    public byte PauseInterval;

    /// <summary>
    ///     TODO actually used in CopyOAM()
    /// </summary>
    public byte Pad;

    /// <summary>
    ///     Current time
    /// </summary>
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

public unsafe struct Struct02000040 {
    public byte Unk00, Unk01;
    public fixed byte Unk02[14];
}

public struct Struct020354C0 {
    public byte Unk0, Unk1;
    public ushort Unk2;
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

    public Span<byte> NameSpan => PointerHelper.ToSpan(ref Name[0], NameCount);
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

public struct Message {
    public byte State, Unk, TextSpeed, Unk3,
        TextWindowWidth, TextWindowHeight,
        TextWindowPosX, TextWindowPosY;

    public ushort TextIndex, Unk2;

    public uint Flags, Rupees;

    public uint Field0x14, Field0x18, Field0x1C;
}

public unsafe struct Token {
    private byte _unk;

    public byte Unk00 {
        readonly get => (byte)((_unk & 0b1000_0000) >>> 7);
        set {
            _unk = (byte)((_unk & 0b0111_1111) | ((value & 1) << 7));
        }
    }

    public byte Unk01 {
        readonly get => (byte)((_unk & 0b0111_1000) >>> 3);
        set {
            _unk = (byte)((_unk & 0b1000_0111) | ((value & 0b1111) << 3));
        }
    }

    public byte Unk05 {
        readonly get => (byte)((_unk & 0b110) >>> 1);
        set {
            _unk = (byte)((_unk & 0b1111_1001) | ((value & 0b11) << 1));
        }
    }

    public byte Unk06 {
        readonly get => (byte)(_unk & 1);
        set {
            _unk = (byte)((_unk & 0b1111_1110) | (value & 1));
        }
    }

    /// <summary>
    ///     First byte read
    /// </summary>
    public byte Code;

    /// <summary>
    ///     Second byte read
    /// </summary>
    public ushort Param;

    /// <summary>
    ///     ASCII adjusted for jp chars
    /// </summary>
    public ushort Extended;

    public ushort _6, TextIndex;

    public void *_C;

    /// <summary>
    ///     Array of 8 pointers to byte
    /// </summary>
    public fixed ulong Buf[8];
}

public struct WStruct {
    private byte _unk;

    public byte Unk00 {
        readonly get => (byte)((_unk & 0b1000_0000) >>> 7);
        set {
            _unk = (byte)((_unk & 0b0111_1111) | ((value & 1) << 7));
        }
    }

    public byte Unk01 {
        readonly get => (byte)((_unk & 0b0111_0000) >>> 4);
        set {
            _unk = (byte)((_unk & 0b1000_1111) | ((value & 0b111) << 4));
        }
    }

    public byte Unk04 {
        readonly get => (byte)(_unk & 0b1111);
        set {
            _unk = (byte)((_unk & 0b1111_0000) | (value & 0b1111));
        }
    }

    public byte Unk1, CharColor, BgColor;
    public ushort Unk4, Unk6;

    /// <summary>
    ///     Pointer to void
    /// </summary>
    public nint Unk8;
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct WordLines {
    [FieldOffset(0)]
    public ushort Word;

    [FieldOffset(0)]
    public byte LineNo;

    [FieldOffset(1)]
    public byte B1;

    [FieldOffset(2)]
    public byte B2;

    [FieldOffset(3)]
    public sbyte SizeScale;
}

public unsafe struct TextRender {
    public Message Message;
    public Token CurrToken;
    public WStruct _50;
    public fixed byte PlayerName[10];
    public fixed byte _66[16];
    public byte _76;
    public fixed byte _77[17];
    public byte MessageStatus, RenderStatus, NewlineDelay,
        _8b, _8c, _8d, _8e, _8f, _90, _91;

    public sbyte TypeSpeed;

    public byte _93, _94, Delay, _96, _97;

    public WordLines _98;

    public byte _9c, UpdateDraw;

    public fixed ushort BeginTiles[4];
    public ushort _a6;
}