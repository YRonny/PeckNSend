using NUnit.Framework.Interfaces;
using PeckNSend.FSM;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class SceneManagerFSM : FiniteStateMachine
        {
            public SceneManagerModel Context { get; }

            public SceneBaseState BootState { get; }
            public SceneBaseState MainMenuState { get; }
            public SceneBaseState PlayerJoinState { get; }
            public SceneBaseState PlayState { get; }
            public SceneBaseState ResultState { get; }

            public SceneManagerFSM(SceneManagerModel context)
            {
                Context = context;

                BootState = new SceneBootState(this);
                MainMenuState = new SceneMainMenuState(this);
                PlayerJoinState = new ScenePlayerJoinState(this);
                PlayState = new ScenePlayState(this);
                ResultState = new SceneResultState(this);
                //TransitionTo(BootState);
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
                CurrentState?.FixedUpdate(fixedDeltaTime);
            }

            public new SceneBaseState CurrentState
            {
                get { return base.CurrentState as SceneBaseState; }
            }
        }
    }
}