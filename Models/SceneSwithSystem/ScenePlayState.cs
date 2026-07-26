using System;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class ScenePlayState : SceneBaseState
        {
            public ScenePlayState(SceneManagerFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ExecuteSceneChange("PlayScene");
            }

            public override void OnExit()
            {
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
            }

            public override void OnRequestResultScene()
            {
                SceneFSM.TransitionTo(SceneFSM.ResultState);
            }

            public override void OnRequestMainMenuScene()
            {
                SceneFSM.TransitionTo(SceneFSM.MainMenuState);
            }

            public override string ToString()
            {
                return "PlayState";
            }
        }
    }
}
