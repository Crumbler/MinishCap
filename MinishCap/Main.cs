using MinishCap.Enums;

namespace MinishCap;

public static class Main {
    public static void AgbMain() {
        Globals.Unk02000010.Field0x4 = 0xC1;

        SetTask(Tasks.Title);
    }

    public static void SetTask(Tasks task) {
        Globals.CurrentTask = task;
        Globals.CurrentTaskState = TaskStates.Transition;
        Globals.CurrentTaskSubState = TaskSubStates.InitRoom;
    }
}