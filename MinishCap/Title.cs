using MinishCap.Enums;

namespace MinishCap;

public static class Title {
    private enum AdvanceState {
        None,
        TimerExpired,
        KeyPressed
    }

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
        IntroState = new();
        Fade.SetFade(FadeType.InOut | FadeType.BlackWhite | FadeType.Instant, 8);
    }

    private static void HandleNintendoCapcomLogos() {
        var advance = GetAdvanceState();

        if (advance == AdvanceState.None) {
            Common.DisplayReset(true);

            IntroState.State = 1;
            IntroState.Timer = 120;
            // LoadGfxGroup 16
            // LoadGfxGroup 1
        } else if (advance == AdvanceState.TimerExpired) {
            advance = AdvanceState.KeyPressed;
        }

        if (advance == AdvanceState.KeyPressed) {
            Globals.Unk02000010.ListenForKeyPresses = true;
            AdvanceIntroSequence(1);
        }
    }

    private static AdvanceState GetAdvanceState() {
        if (Globals.FadeControl.Active) {
            return AdvanceState.None;
        }

        bool newKeys;

        if (!Globals.Unk02000010.ListenForKeyPresses) {
            newKeys = false;
        } else {
            newKeys = false;
        }

        IntroState.Timer--;

        if (IntroState.Timer == 0) {
            return AdvanceState.TimerExpired;
        }

        if (newKeys) {
            return AdvanceState.KeyPressed;
        }

        return AdvanceState.None;
    }

    private static void HandleTitleScreen() {

    }

    private static void ExitTitleScreen() {

    }
}