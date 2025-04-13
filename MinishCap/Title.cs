using MinishCap.Enums;

namespace MinishCap;

public static class Title {
    private class IntroStateClass {
        public int Filler0;
        public byte Language, State, SubState;
        public byte Filler7;
        public ushort Timer;
        public int FillerA1;
        public short FillerA2;
        public byte LightRaysPaletteGroup,
            LightRaysAlphaBlendIndex;
        public byte Counter, Filler13;
        public int SwordBgScaleRatio;
    }

    private static IntroStateClass IntroState = new();

    private static readonly Action[] IntroSequenceHandlers = [
        HandleNintendoCapcomLogos,
        HandleTitleScreen,
        ExitTitleScreen
    ];

    public static void TitleTask() {
        switch (Globals.Main.CurrentTaskState) {
            case TaskStates.Transition:
                Messages.MessageInitialize();
                Globals.UI = new();
                AdvanceIntroSequence(0);
                break;

            case TaskStates.Init:
                IntroSequenceHandlers[Globals.UI.LastState]();
                break;

            case TaskStates.Main:
                if (Globals.FadeControl.Active) {
                    return;
                }

                Common.DisplayReset(true);
                Globals.Main.CurrentTaskState = TaskStates.Init;
                break;
        }

        Affine.CopyOAM();
    }

    private static void AdvanceIntroSequence(int transition) {
        Globals.UI.LastState = (byte)transition;
        Globals.Main.CurrentTaskState = TaskStates.Main;

    }

    private static void HandleNintendoCapcomLogos() {

    }

    private static void HandleTitleScreen() {

    }

    private static void ExitTitleScreen() {

    }
}