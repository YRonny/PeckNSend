using System;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class ScenePlayerJoinState : SceneBaseState
        {
            public ScenePlayerJoinState(SceneManagerFSM fsm) : base(fsm)
            {

            }

            protected virtual void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
            }

            public override void OnEnter()
            {
                Context.ExecuteSceneChange("PlayerJoinScene");
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
                return "PlayerJoinState";
            }
        }
    }
}
