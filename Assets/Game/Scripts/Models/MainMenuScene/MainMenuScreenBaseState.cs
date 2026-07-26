using PeckNSend.FSM;

namespace PeckNSend.Models
{
    public partial class MainMenuScreenModel
    {
        public class MainMenuScreenBaseState : IState
        {
            // FSM
            public MainMenuScreenFSM FSM { get; private set; }
            public MainMenuScreenModel Context => FSM.Context;
            public MainMenuScreenBaseState(MainMenuScreenFSM fsm)
            {
                FSM = fsm;
            }

            // IState methods
            public virtual void Update(float deltaTime) { }
            public virtual void FixedUpdate(float fixedDeltaTime) { }
            public virtual void OnEnter() { }
            public virtual void OnExit() { }


            public virtual void OnRequestAbout() { }
            public virtual void OnRequestSettings() { }
            public virtual void OnRequestHome() { }
            public virtual void OnRequestExit() { }
        }
    }
}
