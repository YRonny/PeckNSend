using System;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel
    {
        public class SceneMainMenuState : SceneBaseState
        {
            public SceneMainMenuState(SceneManagerFSM fsm) : base(fsm)
            {

            }

            protected virtual void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
            }

            public override void OnEnter()
            {
                Context.ExecuteSceneChange("MainMenuScene");
            }

            public override void OnExit()
            {
            }

            public override void FixedUpdate(float fixedDeltaTime)
            {
            }

            public override void OnRequestPlayerJoinScene()
            {
                SceneFSM.TransitionTo(SceneFSM.PlayerJoinState);
            }

            public override void OnRequestQuit()
            {
                Context.ExecuteQuit();
            }

            public override string ToString()
            {
                return "MainMenuState";
            }
        }
    }
}
