namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenMatchFinishedState : PlayScreenBaseState
        {
            public PlayScreenMatchFinishedState(PlayScreenFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ActiveScreen = PlayScreen.MatchFinished;
                Context.CountdownText = string.Empty;
                Context.UpdateMatchTimeText();
                Context.HandleMatchFinished();
            }

            public override string ToString()
            {
                return "PlayScreenMatchFinishedState";
            }
        }
    }
}