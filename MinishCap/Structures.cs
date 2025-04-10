﻿using MinishCap.Enums;
using MinishCap.Extensions;
using MinishCap.Helpers;
using System.Runtime.CompilerServices;
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
        readonly get => _unk.GetBits(7, 1);
        set => _unk = _unk.SetBits(value, 7, 1);
    }

    public byte Unk01 {
        readonly get => _unk.GetBits(3, 4);
        set => _unk = _unk.SetBits(value, 3, 4);
    }

    public byte Unk05 {
        readonly get => _unk.GetBits(1, 2);
        set => _unk = _unk.SetBits(value, 1, 2);
    }

    public byte Unk06 {
        readonly get => _unk.GetBits(0, 1);
        set => _unk = _unk.SetBits(value, 0, 1);
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

public unsafe struct WStruct {
    private byte _unk;

    public byte Unk00 {
        readonly get => _unk.GetBits(7, 1);
        set => _unk = _unk.SetBits(value, 7, 1);
    }

    public byte Unk01 {
        readonly get => _unk.GetBits(4, 3);
        set => _unk = _unk.SetBits(value, 4, 3);
    }

    public byte Unk04 {
        readonly get => _unk.GetBits(0, 4);
        set => _unk = _unk.SetBits(value, 0, 4);
    }

    public byte Unk1, CharColor, BgColor;
    public ushort Unk4, Unk6;

    public void* Unk8;
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

public struct OamData {
    public byte Y;

    private byte _unk1;

    public byte AffineMode {
        readonly get => _unk1.GetBits(6, 2);
        set => _unk1 = _unk1.SetBits(value, 6, 2);
    }

    public byte ObjMode {
        readonly get => _unk1.GetBits(4, 2);
        set => _unk1 = _unk1.SetBits(value, 4, 2);
    }

    public byte Mosaic {
        readonly get => _unk1.GetBits(3, 1);
        set => _unk1 = _unk1.SetBits(value, 3, 1);
    }

    public byte Bpp {
        readonly get => _unk1.GetBits(2, 1);
        set => _unk1 = _unk1.SetBits(value, 2, 1);
    }

    public byte Shape {
        readonly get => _unk1.GetBits(0, 2);
        set => _unk1 = _unk1.SetBits(value, 0, 2);
    }

    private ushort _unk2;

    public ushort X {
        readonly get => _unk2.GetBits(7, 9);
        set => _unk2 = _unk2.SetBits(value, 7, 9);
    }

    /// <summary>
    ///     Bits 3/4 are h-flip/v-flip if not in affine mode
    /// </summary>
    public ushort MatrixNum {
        readonly get => _unk2.GetBits(2, 5);
        set => _unk2 = _unk2.SetBits(value, 2, 5);
    }

    public ushort Size {
        readonly get => _unk2.GetBits(0, 2);
        set => _unk2 = _unk2.SetBits(value, 0, 2);
    }

    private ushort _unk3;

    public ushort TileNum {
        readonly get => _unk3.GetBits(6, 10);
        set => _unk3 = _unk3.SetBits(value, 6, 10);
    }

    public ushort Priority {
        readonly get => _unk3.GetBits(4, 2);
        set => _unk3 = _unk3.SetBits(value, 4, 2);
    }

    public ushort PaletteNum {
        readonly get => _unk3.GetBits(0, 4);
        set => _unk3 = _unk3.SetBits(value, 0, 4);
    }

    public ushort AffineParam;
}

public struct OamObj {
    public byte Unk0, Unk1;
    public ushort Unk2;
    public byte Unk4, Unk5, Unk6, Unk7;
}

public class OamControls {
    public byte Field0x0, Field0x1, SpritesOffset,
        Updated;

    public ushort _4, _6;

    public byte[] Arr0 = new byte[0x18];

    public OamData[] Oam = new OamData[0x80];

    public OamObj[] Unk = new OamObj[0xA0];

    public OamControls() { }
}

public unsafe class UI {
    public byte NextToLoad, _1, LastState, Field0x3,
        State, Field0x5;

    /// <summary>
    ///     Used in Subtask_FadeOut to determine the loadGfx parameter of RestoreGameTask.
    /// </summary>
    public bool LoadGfxOnRestore;

    public byte PauseFadeIn;
    public ushort FadeType, FadeInTime;
    public byte ControlMode, UnkD, UnkE, UnkF;
    public void** CurrentRoomProperties;
    public BgSettings* MapBottomBgSettings, MapTopBgSettings;
    // RoomControls
    // GfxSlotList
    // Palettes
    public byte[] Unk2a8 = new byte[0x100];
    // ActiveScriptInfo
}

public struct RoomControls {

}

public struct GfxSlotList {

}

public struct Palette {

}

public struct ActiveScriptInfo {

}

public struct Entity {

}