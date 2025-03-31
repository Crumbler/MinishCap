using MinishCap.Enums;

namespace MinishCap.Structures;

public struct StructMain {
    public Tasks CurrentTask;
    public TaskStates CurrentTaskState;
    public TaskSubStates CurrentTaskSubState;
    public byte MainField0x5;
    public byte MainField0x7;
    public int Ticks;
}

public unsafe struct Struct02000010 {
    public int Signature;
    public byte Field0x4;
    public byte ListenForKeyPresses;
    public byte Field0x6;
    public byte Field0x7;
    public fixed byte Padding[24];
}