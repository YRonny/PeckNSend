namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel
    {
        public class MainMenuScreenAboutState : MainMenuScreenBaseState
        {
            public MainMenuScreenAboutState(MainMenuScreenFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ActiveScreen = MenuScreen.About;
            }

            public override void OnRequestHome() => FSM.TransitionTo(FSM.HomeState);

            public override string ToString()
            {
                return "MainMenuScreenAboutState";
            }
        }
    }
}