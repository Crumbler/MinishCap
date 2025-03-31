using MinishCap.Enums;

namespace MinishCap;

public static class Main {
    public static void AgbLoad() {
        Save.InitSaveData();

        Globals.Unk02000010.Field0x4 = 0xC1;

        SetTask(Tasks.Title);
    }

    public static void AgbUpdate() {
        HandleCurrentTask();
    }

    public static void AgbRender() {

    }

    public static void SetTask(Tasks task) {
        Globals.Main.CurrentTask = task;
        Globals.Main.CurrentTaskState = TaskStates.Transition;
        Globals.Main.CurrentTaskSubState = TaskSubStates.InitRoom;
    }

    private static void HandleCurrentTask() {

    }
}