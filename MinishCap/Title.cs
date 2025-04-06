using MinishCap.Enums;

namespace MinishCap;

public static class Title {
    private static readonly Action[] IntroSequenceHandlers = [
        HandleNintendoCapcomLogos,
        HandleTitleScreen,
        ExitTitleScreen
    ];

    public static void TitleTask() {
        switch (Globals.Main.CurrentTaskState) {
            case TaskStates.Transition:
                Messages.MessageInitialize();
                break;

            case TaskStates.Init:
                break;

            case TaskStates.Main:
                break;
        }

        Affine.CopyOAM();
    }

    private static void HandleNintendoCapcomLogos() {

    }

    private static void HandleTitleScreen() {

    }

    private static void ExitTitleScreen() {

    }
}