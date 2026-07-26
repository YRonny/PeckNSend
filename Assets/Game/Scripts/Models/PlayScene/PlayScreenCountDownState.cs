namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenCountDownState : PlayScreenBaseState
        {
            public PlayScreenCountDownState(PlayScreenFSM fsm) : base(fsm) { }

            public override void OnEnter()
            {
                Context.ActiveScreen = PlayScreen.CountDown;
                Context.UpdateCountdownText();
                Context.UpdateMatchTimeText();
            }

            public override void Update(float deltaTime)
            {
                Context.PregameCountdownRemaining -= deltaTime;

                if (Context.PregameCountdownRemaining <= 0f)
                {
                    Context.PregameCountdownRemaining = 0f;
                    FSM.TransitionTo(FSM.PlayState);
                }

                Context.UpdateCountdownText();          
            }

            public override void OnRequestPause()
            {
                FSM.StateBeforePause = this;
                FSM.TransitionTo(FSM.PauseState);
            }

            public override string ToString() => "PlayScreenCountDownState";
        }
    }
}