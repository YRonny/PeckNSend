namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenPlayState : PlayScreenBaseState
        {
            public PlayScreenPlayState(PlayScreenFSM fsm) : base(fsm) { }

            public override void OnEnter()
            {
                Context.ActiveScreen = PlayScreen.Play;
                Context.CountdownText = string.Empty;
                Context.UpdateMatchTimeText();
            }

            public override void Update(float deltaTime)
            {
                Context.MatchTimeRemaining -= deltaTime;

                if (Context.MatchTimeRemaining <= 0f)
                {
                    Context.MatchTimeRemaining = 0f;
                    Context.UpdateMatchTimeText();
                    FSM.TransitionTo(FSM.MatchFinishedState);
                }

                Context.UpdateMatchTimeText();
            }

            public override void OnRequestPause()
            {
                FSM.StateBeforePause = this;
                FSM.TransitionTo(FSM.PauseState);
            }

            public override string ToString() => "PlayScreenPlayState";
        }
    }
}