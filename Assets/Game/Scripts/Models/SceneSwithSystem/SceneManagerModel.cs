using System;
using System.Numerics;
using UnityEngine.SceneManagement;

namespace PeckNSend.Models
{
    public partial class SceneManagerModel : UnityModelBaseClass
    {
        public SceneManagerFSM SceneFSM { get; }

        public SceneManagerModel()
        {
            SceneFSM = new SceneManagerFSM(this);
        }

        public override void FixedUpdate(float fixedDeltaTime)
        {
            base.FixedUpdate(fixedDeltaTime);

            SceneFSM.FixedUpdate(fixedDeltaTime);
        }

        #region ------REQUESTS------

        public void RequestBoot()
        {
            SceneFSM.TransitionTo(SceneFSM.BootState);
        }

        public void RequestMainMenuScene()
        {
            SceneFSM.CurrentState?.OnRequestMainMenuScene();
        }

        public void RequestPlayerJoinScene()
        {
            SceneFSM.CurrentState?.OnRequestPlayerJoinScene();
        }

        public void RequestPlayScene()
        {
            SceneFSM.CurrentState?.OnRequestPlayScene();
        }

        public void RequestResultScene()
        {
            SceneFSM.CurrentState?.OnRequestResultScene();
        }

        public void RequestQuit()
        {
            SceneFSM.CurrentState?.OnRequestQuit();
        }


        #endregion

        #region ------EXECUTES------

        public void ExecuteSceneChange(string sceneName)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == sceneName)
            {
                return;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void ExecuteQuit()
        {
            //Application.Quit();
        }

        #endregion
    }
}
