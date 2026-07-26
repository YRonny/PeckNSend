using PeckNSend.FSM;

namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel
    {
        public class MainMenuScreenFSM : FiniteStateMachine
        {

            public MainMenuScreenModel Context { get; set; }

            public MainMenuScreenBaseState HomeState { get; private set; }
            public MainMenuScreenBaseState SettingState { get; private set; }
            public MainMenuScreenBaseState AboutState { get; private set; }

            //potentially charachter loadout state

            /// <summary>
            /// Tracks the state active before pausing so we can return to it on resume.
            /// Assigned by individual states in OnRequestPause.
            /// </summary>
            //public MainMenuScreenBaseState StateBeforePause { get; internal set; }

            public new MainMenuScreenBaseState CurrentState
            {
                get { return base.CurrentState as MainMenuScreenBaseState; }
            }

            public MainMenuScreenFSM(MainMenuScreenModel context)
            {
                Context = context;

                HomeState = new MainMenuScreenHomeState(this);
                SettingState = new MainMenuScreenSettingState(this);
                AboutState = new MainMenuScreenAboutState(this);
                TransitionTo(HomeState);
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
                CurrentState?.FixedUpdate(fixedDeltaTime);
            }
        }
    }
}
        