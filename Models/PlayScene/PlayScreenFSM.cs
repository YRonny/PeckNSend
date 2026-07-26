using PeckNSend.FSM;

namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenFSM : FiniteStateMachine
        {

            public PlayScreenModel Context { get; set; }

            public PlayScreenBaseState PlayState { get; private set; }
            public PlayScreenBaseState PauseState { get; private set; }
            public PlayScreenBaseState CountDownState { get; private set; }
            public PlayScreenBaseState MatchFinishedState { get; private set; }

            /// <summary>
            /// Tracks the state active before pausing so we can return to it on resume.
            /// Assigned by individual states in OnRequestPause.
            /// </summary>
            public PlayScreenBaseState StateBeforePause { get; internal set; }

            public new PlayScreenBaseState CurrentState
            {
                get { return base.CurrentState as PlayScreenBaseState; }

            }

            public PlayScreenFSM(PlayScreenModel context)
            {
                Context = context;

                PlayState = new PlayScreenPlayState(this);
                PauseState = new PlayScreenPauseState(this);
                CountDownState = new PlayScreenCountDownState(this);
                MatchFinishedState = new PlayScreenMatchFinishedState(this);
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
                CurrentState?.FixedUpdate(fixedDeltaTime);
            }
        }
    }
}
        