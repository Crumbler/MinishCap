using MinishCap.Enums;
using MinishCap.Structures;

namespace MinishCap;

public static class Main {
    private const Languages GameLanguage = Languages.English;
    private const int TotalSaveSlots = 3;
    private const int MaximumMessageSpeed = 3;
    private const int MaximumBrightness = 3;
    private static readonly SaveHeader DefaultSaveHeader = CreateDefaultSaveHeader();

    private static SaveHeader CreateDefaultSaveHeader() {
        var header = new SaveHeader() {
            Signature = Constants.FileSignature,
            SaveFileId = 0,
            MsgSpeed = 1,
            Brightness = 1,
            Language = GameLanguage,
            Invalid = 0,
            Initialized = 0
        };

        "LINK"u8.CopyTo(header.NameSpan);

        return header;
    }

    public static void AgbLoad() {
        Save.InitSaveData();
        InitSaveHeader();
        Save.Flush();

        Globals.Unk02000010.Field0x4 = 0xC1;

        SetTask(Tasks.Title);
    }

    public static void AgbUpdate() {
        HandleCurrentTask();
    }

    public static void AgbRender() {

    }

    private static void InitSaveHeader() {
        if (!CheckHeaderValid()) {
            int res = Save.ReadSaveHeader(ref Globals.SaveHeader);
            switch (res) {
                case 1:
                    if (CheckHeaderValid()) {
                        break;
                    }

                    goto case -1;
                case 0:
                case -1:
                default:
                    Globals.SaveHeader = DefaultSaveHeader;
                    Save.WriteSaveHeader(Globals.SaveHeader);
                    break;
            }
        }

        bool resetUnk = (Globals.Unk02000010.Signature ^ Constants.FileSignature) != 0;

        if (Globals.Unk02000010.Field0x4 is not 0 and not 0xC1) {
            resetUnk = true;
        }

        if (resetUnk) {
            Globals.Unk02000010 = new() {
                Signature = Constants.FileSignature
            };
        }
    }

    private static bool CheckHeaderValid() {
        if (Globals.SaveHeader.Signature != Constants.FileSignature ||
            Globals.SaveHeader.SaveFileId >= TotalSaveSlots ||
            Globals.SaveHeader.MsgSpeed >= MaximumMessageSpeed ||
            Globals.SaveHeader.Brightness >= MaximumBrightness ||
            Globals.SaveHeader.Language != GameLanguage ||
            Globals.SaveHeader.Invalid != 0) {
            return false;
        }

        return true;
    }

    public static void SetTask(Tasks task) {
        Globals.Main.CurrentTask = task;
        Globals.Main.CurrentTaskState = TaskStates.Transition;
        Globals.Main.CurrentTaskSubState = TaskSubStates.InitRoom;
    }

    private static void HandleCurrentTask() {

    }
}