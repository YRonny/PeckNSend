using System;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class SceneResultState : SceneBaseState
        {
            public SceneResultState(SceneManagerFSM fsm) : base(fsm)
            {
            }

            public override void OnEnter()
            {
                Context.ExecuteSceneChange("ResultScene");
            }

            public override void OnExit()
            {
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
            }

            public override void OnRequestMainMenuScene()
            {
                SceneFSM.TransitionTo(SceneFSM.MainMenuState);
            }

            public override void OnRequestPlayScene()
            {
                SceneFSM.TransitionTo(SceneFSM.PlayState);
            }

            public override string ToString()
            {
                return "ResultState";
            }
        }
    }
}
