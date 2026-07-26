using PeckNSend.FSM;

namespace PeckNSend.Models
{
    public partial class PlayScreenModel
    {
        public class PlayScreenBaseState : IState
        {
            // FSM
            public PlayScreenFSM FSM { get; private set; }
            public PlayScreenModel Context => FSM.Context;
            public PlayScreenBaseState(PlayScreenFSM fsm)
            {
                FSM = fsm;
            }

            // IState methods
            public virtual void Update(float deltaTime) { }
            public virtual void FixedUpdate(float fixedDeltaTime) { }
            public virtual void OnEnter() { }
            public virtual void OnExit() { }


            public virtual void OnRequestPause() { }
            public virtual void OnRequestResume() { }
        }
    }
}
