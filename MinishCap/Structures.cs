using MinishCap.Enums;
using MinishCap.Extensions;
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

public unsafe struct GfxSlot {
    private byte _val;

    public byte Status {
        readonly get => BitfieldExtensions.GetBits(_val, 4, 4);
        set => _val = BitfieldExtensions.SetBits(_val, value, 4, 4);
    }

    public byte VramStatus {
        readonly get => BitfieldExtensions.GetBits(_val, 0, 4);
        set => _val = BitfieldExtensions.SetBits(_val, value, 0, 4);
    }

    public byte SlotCount;

    /// <summary>
    ///     How many entities use this gfx slot
    /// </summary>
    public byte ReferenceCount;

    public byte Unk3;

    public ushort GfxIndex, PaletteIndex;

    public void* PalettePointer;
}

public unsafe struct GfxSlotList {
    public byte Unk0, Unk1, Unk2, Unk3;

    // Times size of GfxSlot
    private fixed byte _gfxSlots[Constants.MaxGfxSlots * 16];

    public Span<GfxSlot> GfxSlots {
        get => PointerHelper.AsSpan<GfxSlot>(ref _gfxSlots[0], Constants.MaxGfxSlots);
    }
}

public struct Palette {
    private byte _val;

    public byte Unk0 {
        readonly get => _val.GetBits(4, 4);
        set => _val = _val.SetBits(value, 4, 4);
    }

    public byte Unk4 {
        readonly get => _val.GetBits(0, 4);
        set => _val = _val.SetBits(value, 0, 4);
    }

    public byte Unk1;
    public ushort ObjPaletteId;
}

public struct ActiveScriptInfo {
    /// <summary>
    ///     Sync flags are used to synchronize scripts running on different entities
    /// </summary>
    public uint SyncFlags;

    public ushort CommandIndex;

    public byte CommandSize, Flags, FadeSpeed;
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
    public RoomControls RoomControls;
    public GfxSlotList GfxSlotList;
    
    public readonly Palette[] Palettes = new Palette[16];

    public byte[] Unk2a8 = new byte[0x100];
    public ActiveScriptInfo ActiveScriptInfo;
}

public unsafe struct RoomControls {
    public short ScrollX, ScrollY;

    /// <summary>
    ///     Pixels per frame that the camera can scroll.
    /// </summary>
    public byte ScrollSpeed;

    /// <summary>
    ///     0x2 = ?? (apply collision value on bottom map no matter the layer SetTileType is
    ///     called for), 0x4 = camera scrolling
    /// </summary>
    public byte ScrollFlags;

    public byte ScrollDirection;

    public sbyte OamOffsetX, OamOffsetY;

    public byte Unk13, ShakeMagnitude, Unk15;

    public ushort ShakeDuration;

    /// <summary>
    ///     progress during transition in same area?
    /// </summary>
    public ushort Unk18;

    /// <summary>
    ///     calculated from unk18
    /// </summary>
    public ushort Unk1A;

    /// <summary>
    ///     0, 0xff
    /// </summary>
    public ushort Unk1C;

    /// <summary>
    ///     Width in pixels.
    /// </summary>
    public ushort Width;

    /// <summary>
    ///     Height in pixels.
    /// </summary>
    public ushort Height;

    /// <summary>
    ///     so far always 0xffff
    /// </summary>
    public ushort Unk22;

    public sbyte AffX, AffY;

    public byte Unk26, Unk27;

    public SplitWord Bg3OffsetX, Bg3OffsetY;

    public Entity* CameraTarget;

    /// <summary>
    ///     TODO Should be MapDataDefinition*, but then LoadRoomTileSet does not match.
    /// </summary>
    public uint TileSet;
}

public struct SpriteSettings {
    private byte _val;

    /// <summary>
    ///     Draw type. 0 = disabled, 1 = clip to screen, 3 = draw always
    /// </summary>
    public byte Draw {
        readonly get => BitfieldExtensions.GetBits(_val, 6, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 7, 2);
    }

    public byte Ss2 {
        readonly get => BitfieldExtensions.GetBits(_val, 5, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 5, 1);
    }

    public byte Ss3 {
        readonly get => BitfieldExtensions.GetBits(_val, 4, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 4, 1);
    }
    
    public byte Shadow {
        readonly get => BitfieldExtensions.GetBits(_val, 2, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 2, 2);
    }

    public byte FlipX {
        readonly get => BitfieldExtensions.GetBits(_val, 1, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 1, 1);
    }

    public byte FlipY {
        readonly get => BitfieldExtensions.GetBits(_val, 0, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 0, 1);
    }
}

public struct SpriteRendering {
    private byte _val;

    public byte B0 {
        readonly get => BitfieldExtensions.GetBits(_val, 6, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 6, 2);
    }

    public byte AlphaBlend {
        readonly get => BitfieldExtensions.GetBits(_val, 4, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 4, 2);
    }

    public byte B2 {
        readonly get => BitfieldExtensions.GetBits(_val, 2, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 2, 2);
    }

    public byte B3 {
        readonly get => BitfieldExtensions.GetBits(_val, 0, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 0, 2);
    }
}

public struct SpritePriority {
    private byte _val;

    public byte B0 {
        readonly get => BitfieldExtensions.GetBits(_val, 5, 3);
        set => _val = BitfieldExtensions.SetBits(_val, value, 5, 3);
    }

    public byte B1 {
        readonly get => BitfieldExtensions.GetBits(_val, 2, 3);
        set => _val = BitfieldExtensions.SetBits(_val, value, 2, 3);
    }

    public byte B2 {
        readonly get => BitfieldExtensions.GetBits(_val, 1, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 1, 1);
    }

    public byte B3 {
        readonly get => BitfieldExtensions.GetBits(_val, 0, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 0, 1);
    }
}

public struct SpriteOrientation {
    private byte _val;

    public byte B0 {
        readonly get => BitfieldExtensions.GetBits(_val, 7, 1);
        set => _val = BitfieldExtensions.SetBits(_val, value, 6, 2);
    }

    public byte B1 {
        readonly get => BitfieldExtensions.GetBits(_val, 2, 5);
        set => _val = BitfieldExtensions.SetBits(_val, value, 2, 5);
    }

    public byte FlipY {
        readonly get => BitfieldExtensions.GetBits(_val, 0, 2);
        set => _val = BitfieldExtensions.SetBits(_val, value, 0, 2);
    }
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct SplitWord {
    [FieldOffset(0)]
    public int Word;

    [FieldOffset(0)]
    public uint WordU;

    [FieldOffset(0)]
    public short HalfLo;

    [FieldOffset(2)]
    public short HalfHi;

    [FieldOffset(0)]
    public ushort HalfULo;

    [FieldOffset(2)]
    public ushort HalfUHi;

    [FieldOffset(0)]
    public byte Byte0;

    [FieldOffset(1)]
    public byte Byte1;

    [FieldOffset(2)]
    public byte Byte2;

    [FieldOffset(3)]
    public byte Byte3;
}

public unsafe struct Hitbox {
    public sbyte OffsetX, OffsetY;

    public fixed byte Unk2[4];

    public byte Width, Height;
}

public unsafe struct Entity {
    public Entity* Prev, Next;
    
    /// <summary>
    ///     See EntityKind
    /// </summary>
    public byte Kind;

    public byte Id;

    /// <summary>
    ///     For use internally
    /// </summary>
    public byte Type, Type2;

    /// <summary>
    ///     Current action. Usually used to index a function table.
    /// </summary>
    public byte Action;

    /// <summary>
    ///     Optional sub-action
    /// </summary>
    public byte SubAction;

    /// <summary>
    ///     General purpose timer
    /// </summary>
    public byte Timer, SubTimer;

    /// <summary>
    ///     See EntityFlags
    /// </summary>
    public byte Flags;

    /// <summary>
    ///     Current priority. @see Priority
    /// </summary>
    public byte UpdatePriority;

    /// <summary>
    ///     Priority to restore after request is over. @see RequestPriority.
    /// </summary>
    public byte UpdatePriorityPrev;

    public short SpriteIndex;

    /// <summary>
    ///     Animation state. @see AnimationState
    /// </summary>
    public byte AnimationState;

    /// <summary>
    ///     Facing direction. @see Direction
    /// </summary>
    public byte Direction;

    /// <summary>
    ///     Flags for carrying this Entity.
    /// </summary>
    public byte CarryFlags;

    /// <summary>
    ///     Controls collisions between followers, unused.
    /// </summary>
    public byte FollowerFlag;

    public SpriteSettings SpriteSettings;
    public SpriteRendering SpriteRendering;
    public byte PaletteRaw;
    public byte PaletteB0 {
        get => BitfieldExtensions.GetBits(PaletteRaw, 4, 4);
        set => PaletteRaw = BitfieldExtensions.SetBits(PaletteRaw, value, 4, 4);
    }
    public byte PaletteB4 {
        get => BitfieldExtensions.GetBits(PaletteRaw, 0, 4);
        set => PaletteRaw = BitfieldExtensions.SetBits(PaletteRaw, value, 0, 4);
    }

    public SpriteOrientation SpriteOrientation;

    /// <summary>
    ///     Controls SFX and other things
    /// </summary>
    public byte GustJarFlags;

    /// <summary>
    ///     Frames needed to pull off ground
    /// </summary>
    public byte GustJarTolerance;

    public byte FrameIndex, LastFrameIndex;

    /// <summary>
    ///     Z axis speed measured in px/frame
    /// </summary>
    public int ZVelocity;

    /// <summary>
    ///     Magnitude of speed
    /// </summary>
    public short Speed;

    public fixed byte SpriteAnimation[3];

    public SpritePriority SpritePriority;

    /// <summary>
    ///     Collision flags for each direction. @see Collisions
    /// </summary>
    public ushort Collisions;

    /// <summary>
    ///     X position, fixed point Q16.16.
    /// </summary>
    public SplitWord X;

    /// <summary>
    ///     Y position, fixed point Q16.16.
    /// </summary>
    public SplitWord Y;

    /// <summary>
    ///     Z position, fixed point Q16.16.
    /// </summary>
    public SplitWord Z;

    /// <summary>
    ///     @see CollisionLayer.
    /// </summary>
    public byte CollisionLayer;

    public sbyte InteractType;

    /// <summary>
    ///     4: grabbed by GustJar
    /// </summary>
    public byte GustJarState;

    /// <summary>
    ///     Bitfield. @see CollisionClass
    /// </summary>
    public byte CollisionMask;

    /// <summary>
    ///     @see CollisionFlags, @see CollisionClass
    /// </summary>
    public byte CollisionFlags;

    /// <summary>
    ///     Invulnerability frames.
    /// </summary>
    public sbyte IFrames;

    /// <summary>
    ///     Direction of knockback.
    /// </summary>
    public byte KnockbackDirection;

    /// <summary>
    ///     Behavior as a collision sender.
    /// </summary>
    public byte HitType;

    /// <summary>
    ///     Behavior as a collision receiver.
    /// </summary>
    public byte HurtType;

    /// <summary>
    ///     Information about collision contact.
    /// </summary>
    public byte ContactFlags;

    /// <summary>
    ///     Duration of knockback.
    /// </summary>
    public byte KnockbackDuration;

    /// <summary>
    ///     Frames that this Entity is confused.
    /// </summary>
    public byte ConfusedTime;

    /// <summary>
    ///     Damage this Entity inflicts.
    /// </summary>
    public byte Damage;

    /// <summary>
    ///     Health of this Entity.
    /// </summary>
    public byte Health;

    /// <summary>
    ///     How fast this Entity is knocked back.
    /// </summary>
    public ushort KnockbackSpeed;

    public Hitbox* Hitbox;

    public Entity* ContactedEntity;

    /// <summary>
    ///     Parent Entity. Sometimes points to associated data.
    /// </summary>
    public Entity* Parent;

    /// <summary>
    ///     Child Entity. Sometimes points to associated data.
    /// </summary>
    public Entity* Child;

    public byte AnimIndex, FrameDuration,
        Frame, FrameSpriteSettings;

    public void* AnimPtr;

    public ushort SpriteVramOffset;

    public byte SpriteOffsetX;
    public sbyte SpriteOffsetY;

    /// <summary>
    ///     Heap data allocated with #zMalloc.
    /// </summary>
    public void* MyHeap;
}