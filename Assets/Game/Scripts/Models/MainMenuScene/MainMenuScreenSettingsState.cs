namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel
    {
        public class MainMenuScreenSettingState : MainMenuScreenBaseState
        {
            public MainMenuScreenSettingState(MainMenuScreenFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ActiveScreen = MenuScreen.Settings;
            }

            public override void OnRequestHome() => FSM.TransitionTo(FSM.HomeState);

            public override string ToString()
            {
                return "MainMenuScreenSettingState";
            }
        }
    }
}