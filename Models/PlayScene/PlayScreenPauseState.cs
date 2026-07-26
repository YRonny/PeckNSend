namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenPauseState : PlayScreenBaseState
        {
            public PlayScreenPauseState(PlayScreenFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ActiveScreen = PlayScreen.Pause;
            }

            public override void OnRequestResume()
            {
                PlayScreenBaseState returnTo = FSM.PlayState;
                FSM.StateBeforePause = null;
                FSM.TransitionTo(returnTo);
            }

            public override string ToString()
            {
                return "PlayScreenPauseState";
            }
        }
    }
}