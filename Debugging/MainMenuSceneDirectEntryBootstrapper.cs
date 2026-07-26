using PeckNSend.Models;
using PeckNSend.Presenters;
using UnityEngine;

namespace PeckNSend.Debugging
{
    [DefaultExecutionOrder(-1000)]
    public class MainMenuSceneDirectEntryBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _enableDirectSceneBootstrap = true;

        private void Awake()
        {
            if (!_enableDirectSceneBootstrap)
            {
                return;
            }

            EnsurePersistentPresenters();
            EnsureSceneManagerIsInMainMenuState();
        }

        private void EnsurePersistentPresenters()
        {
            GameObject appRoot = GameObject.Find("__AppRoot");

            if (appRoot == null)
            {
                appRoot = new GameObject("__AppRoot");
            }

            if (SceneManagerPresenter.Instance == null)
            {
                appRoot.AddComponent<SceneManagerPresenter>();
            }

            if (GameSessionPresenter.Instance == null)
            {
                appRoot.AddComponent<GameSessionPresenter>();
            }
        }

        private void EnsureSceneManagerIsInMainMenuState()
        {
            if (SceneManagerPresenter.Instance == null || SceneManagerPresenter.Instance.Model == null)
            {
                Debug.LogError("SceneManagerPresenter or SceneManager model could not be created.");
                return;
            }

            SceneManagerModel sceneManagerModel = SceneManagerPresenter.Instance.Model;

            if (sceneManagerModel.SceneFSM.CurrentState == null)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.MainMenuState);
                return;
            }

            if (sceneManagerModel.SceneFSM.CurrentState != sceneManagerModel.SceneFSM.MainMenuState)
            {
                sceneManagerModel.SceneFSM.TransitionTo(sceneManagerModel.SceneFSM.MainMenuState);
            }
        }
    }
}