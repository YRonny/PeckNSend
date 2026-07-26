namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel
    {
        public class MainMenuScreenHomeState : MainMenuScreenBaseState
        {
            public MainMenuScreenHomeState(MainMenuScreenFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ActiveScreen = MenuScreen.Home;
            }

            public override void OnRequestAbout() => FSM.TransitionTo(FSM.AboutState);
            public override void OnRequestSettings() => FSM.TransitionTo(FSM.SettingState);
            public override void OnRequestExit() => Context.ExecuteExit();

            public override string ToString()
            {
                return "MainMenuScreenHomeState";
            }
        }
    }
}